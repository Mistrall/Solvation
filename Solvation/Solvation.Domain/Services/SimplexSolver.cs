﻿using System.Collections.Generic;
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

		internal Vector SolveInternal(Matrix A, Vector b, Vector c, int[] B, Matrix AB, Vector bB)
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
				int e=0;
				var v = double.MaxValue;
				foreach (var k in K)
				{
					var w = (b[k] - A.Row(k)*x)/(A.Row(k)*d);
					if (!(w < v)) continue;
					v = w;
					e = k;
				}

				//Update basis
				B[r] = e;
				AB.SetRow(r, A.Row(e));
				bB[r] = b[e];

				//Trick - lets update inverse AB in a smart way - sinse there is only one new inequality we only need to 
				//compute new inversed row (should drop complexity of whole algo to n*n)
				var f = AB.Row(r)*ABi;

				var g = -f;
				g[r] = 1;
				g/= f[r];

				g[r] -= 1;

				ABi = ABi.Add(ABi.Column(r).ToColumnMatrix() * g.ToRowMatrix());
				//Compute new x
				x = x + v*d;
			}
		}

		public SimplexResult Solve(SimplexTuple tuple, int[] B)
		{
			var standartTuple = (new SimplexInputBuilder()).ConvertToStandartForm(tuple);
			iteration = 0;
			var AB = standartTuple.EqualityCoeffs.Extract(B, Enumerable.Range(0, standartTuple.EqualityCoeffs.ColumnCount).ToArray());
			var bB = standartTuple.FreeTerms.Extract(B);

			var vector = SolveInternal(standartTuple.EqualityCoeffs, standartTuple.FreeTerms, standartTuple.ObjFuncCoeffs, B, AB, bB);
			var optimalValue = (standartTuple.ObjFuncCoeffs.ToRowMatrix() * vector)[0];

			return new SimplexResult {OptimalValue = optimalValue, OptimalVector = vector, Iteration = iteration};
		}
	}
}