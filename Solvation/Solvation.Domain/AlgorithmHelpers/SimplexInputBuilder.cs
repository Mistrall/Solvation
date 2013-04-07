using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.DomainObjects.Simplex;

namespace Solvation.Domain.AlgorithmHelpers
{
	public class SimplexInputBuilder
	{
		public SimplexTuple BuildFromBasePlan(List<PlanStep> basePlan)
		{
			//Build objective function (minimize total time)
			var type = ObjectiveFunctionType.Min;
			var objFunctionCoeffs = new LinkedList<double>();
			var eqCoeffs = new LinkedList<LinkedList<double>>();
			var freeTerms = new LinkedList<double>();
			foreach (var planStep in basePlan)
			{
				foreach (var executingJob in planStep.ExecutingJobs)
				{
					//Build Objecttive function coeffs
					objFunctionCoeffs.AddLast(1/executingJob.Intencity);
					//Build eqCoeff matrix (A)
					//Build free terms (b)
				}
			}


			return new SimplexTuple(new double[,]{}, new double[]{}, new double[]{}, 0);
		}

		public SimplexTuple ConvertToStandartForm(SimplexTuple tuple)
		{
			if (tuple.Type == ObjectiveFunctionType.Min)
			{
				tuple.ObjFuncCoeffs *= -1;
				tuple.ObjFuncFreeTerm *= -1;
				tuple.Type = ObjectiveFunctionType.Max;
			}
			int length = tuple.EquationTypes.Count;
			for (int i = 0; i < length; i++)
			{
				var equation = tuple.EquationTypes[i];
				if (equation == EquationType.MoreOrEqual)
				{
					tuple.EqualityCoeffs.SetRow(i, -1 * tuple.EqualityCoeffs.Row(i));
					tuple.FreeTerms[i] *= -1;
					tuple.EquationTypes[i] = EquationType.LessOrEqual;
				}
				if (equation == EquationType.Equal)
				{

					tuple.EquationTypes[i] = EquationType.LessOrEqual;
					var additionalRow = tuple.EqualityCoeffs.Row(i).Clone();
					tuple.EqualityCoeffs=(DenseMatrix)tuple.EqualityCoeffs.InsertRow(i + 1, additionalRow);
					tuple.EquationTypes.Insert(i + 1, EquationType.MoreOrEqual);
					var val = tuple.FreeTerms[i];
					var freeTerms = tuple.FreeTerms.ToList();
					freeTerms.Insert(i + 1, val);
					tuple.FreeTerms = new DenseVector(freeTerms.ToArray());
					length++;
				}
			}

			return tuple;
		}
	}
}
