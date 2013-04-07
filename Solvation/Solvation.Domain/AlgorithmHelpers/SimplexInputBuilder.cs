using System.Collections.Generic;
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
	}
}
