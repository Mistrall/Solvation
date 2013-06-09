using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects.Simplex;
using Solvation.Domain.Extensions;

namespace Solvation.Domain.Services
{
	public class SimplexSolver
	{
		private int iteration;

		internal Vector SolveInternal(Matrix A, Vector b, Vector c, StartingBasis B, Matrix AB, Vector bB)
		{
			//Init simplex
			var m = A.RowCount;

			var ABi = AB.Inverse();
			//X is a starting vector
			var x = AB.LU().Solve(bB);

			while (true)
			{
				iteration++;
				//Compute lambda (cT*AB.inv())T
				var lambda = (c.ToRowMatrix()*ABi).Transpose().ToRowWiseArray();

				//check for optimality
				if (lambda.All(l => l >= 0)) return (Vector) x;

				//Find leaving index r (first index where component < 0)
				var r = lambda.Select((i, index) => new {i, index})
				              .Where((i, index) => i.i < 0)
				              .First().index;

				//compute direction to move int - take r-th column
				var d = ABi.Column(r)*(-1);

				//Determine the set K (all indexes of positive values of lambda)
				//all k that a(k).T*d>0, 1 <= i <=m
				var K = new List<int>();
				for (int k = 0; k < m; k++)
				{
					var val = A.Row(k)*d;

					if (val > 0 && !val.FloatEquals(0))
						K.Add(k);
				}

				if (K.Count == 0)
					throw new SimplexException("Problem is unbounded") {Iteration = iteration};

				//Find entering index e
				int e = 0;
				var v = double.MaxValue;
				foreach (var k in K)
				{
					var w = (b[k] - A.Row(k)*x)/(A.Row(k)*d);
					if (!(w < v)) continue;
					v = w;
					e = k;
				}

				//Update basis
				B.InequalityIndexes[r] = e;
				AB.SetRow(r, A.Row(e));
				bB[r] = b[e];

				//Trick - lets update inverse AB in a smart way - sinse there is only one new inequality we only need to 
				//compute new inversed row (should drop complexity of whole algo to n*n)
				var f = AB.Row(r)*ABi;

				var g = -f;
				g[r] = 1;
				g /= f[r];

				g[r] -= 1;

				ABi = ABi.Add(ABi.Column(r).ToColumnMatrix()*g.ToRowMatrix());
				//Compute new x
				x = x + v*d;
			}
		}

		public SimplexResult Solve(SimplexTuple tuple)
		{
			var standartTuple = (new SimplexInputBuilder()).ConvertToStandartForm(tuple);
			var multiplier = 1;
			if (tuple.Type == ObjectiveFunctionType.Min) multiplier *= -1;
			var bInternal = InitializeSimplex(tuple);

			iteration = 0;
			var columns = Enumerable.Range(0, tuple.ObjFuncCoeffs.Count).ToArray();
			var AB = standartTuple.EqualityCoeffs.Extract(bInternal.InequalityIndexes, columns);
			var bB = standartTuple.FreeTerms.Extract(bInternal.InequalityIndexes);

			var vector = SolveInternal(standartTuple.EqualityCoeffs, standartTuple.FreeTerms, standartTuple.ObjFuncCoeffs,
			                           bInternal, AB, bB);
			//Multiply result to -1 if we have min function at start
			var optimalValue = multiplier*(standartTuple.ObjFuncCoeffs.ToRowMatrix()*vector)[0];

			return new SimplexResult {OptimalValue = optimalValue, OptimalVector = vector, Iteration = iteration};
		}

		internal StartingBasis InitializeSimplex(SimplexTuple tuple)
		{
			var negativeCount = tuple.FreeTerms.Count(x => x < 0);
			int varCount = tuple.ObjFuncCoeffs.Count;

			var startingEq = new List<int>(varCount);
			for (int i = 0; i < tuple.EqualityCoeffs.RowCount; i++)
			{
				var row = tuple.EqualityCoeffs.Row(i);
				if (row.Count(x => x.FloatEquals(-1)) == 1
				    && row.Count(x => x.FloatEquals(0)) == (varCount - 1)
				    && startingEq.Count < varCount)
					startingEq.Add(i);
			}
			//If min index > 0 -> all free terms are positive.
			//This means that basis just zero variables and indexes of that inequalities
			if (negativeCount == 0)
			{
				return new StartingBasis
					{
						InequalityIndexes = startingEq.ToArray(),
						FeasibleBasis = new double[varCount]
					};
			}

			//Create auxilary problem 
			var auxObjFunc = new List<double>(varCount + negativeCount);
			auxObjFunc.AddRange(tuple.ObjFuncCoeffs.Select(ofc => 0).Select(dummy => (double) dummy));

			var auxA = tuple.EqualityCoeffs.Clone();
			var auxb = tuple.FreeTerms.Clone();
			int rowCount = tuple.EqualityCoeffs.RowCount;
			//for each negative index we should go with added slack variable
			//TODO: work with Dense/Sparse vectors
			for (int i = 0; i < tuple.EqualityCoeffs.RowCount; i++)
			{
				if (tuple.FreeTerms[i] >= 0) continue;
				//add a new slack variable to obj function with coeff -1
				auxObjFunc.Add(-1);
				//add new column for that variable
				var zeroVector = new DenseVector(rowCount);
				zeroVector[i] = -1;
				auxA = auxA.InsertColumn(varCount++, zeroVector);
				//add new row for this variable
				var nonNegativeRow = new DenseVector(varCount);
				nonNegativeRow[varCount - 1] = -1;
				auxA = auxA.InsertRow(rowCount, nonNegativeRow);
				var zeroFreeTerm = new DenseVector(new[] {0.0});
				auxb = auxb.ToColumnMatrix().InsertRow(rowCount, zeroFreeTerm).Column(0);
				rowCount++;
				//add top constraint to this variable
				var freeTermRow = new DenseVector(varCount);
				freeTermRow[varCount - 1] = 1;
				auxA = auxA.InsertRow(rowCount, freeTermRow);
				var newFreeTerm = new DenseVector(new[] {-tuple.FreeTerms[i]});
				auxb = auxb.ToColumnMatrix().InsertRow(rowCount, newFreeTerm).Column(0);
				startingEq.Add(rowCount);
				rowCount++;
			}

			var auxBasis = new StartingBasis
				{
					InequalityIndexes = startingEq.ToArray(),
					FeasibleBasis = new double[varCount]
				};

			iteration = 0;
			var columns = Enumerable.Range(0, varCount).ToArray();
			//TODO: work with Dense/Sparse vectors
			var auxAb = ((DenseMatrix) auxA).Extract(startingEq.ToArray(), columns);
			var auxbB = ((DenseVector) auxb).Extract(startingEq.ToArray());

			Vector vector = SolveInternal((DenseMatrix) auxA, (DenseVector) auxb, new DenseVector(auxObjFunc.ToArray()),
			                              auxBasis, auxAb, auxbB);

			for (int k = tuple.ObjFuncCoeffs.Count; k < varCount; k++)
				if (!vector[k].FloatEquals(0))
					throw new SimplexException("Problem is unsolvable");

			var basis = vector.Take(tuple.ObjFuncCoeffs.Count).ToArray();
			var indexes =
				auxBasis.InequalityIndexes.Where(x => x < tuple.EqualityCoeffs.RowCount).Take(tuple.ObjFuncCoeffs.Count).ToArray();

			return new StartingBasis
				{
					InequalityIndexes = indexes,
					FeasibleBasis = basis
				};
		}
	}
}