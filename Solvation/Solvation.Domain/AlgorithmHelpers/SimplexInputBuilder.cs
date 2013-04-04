using System.Collections.Generic;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.DomainObjects.Simplex;

namespace Solvation.Domain.AlgorithmHelpers
{
	public class SimplexInputBuilder
	{
		public SimplexTuple BuildFromBasePlan(List<PlanStep> basePlan)
		{
			return new SimplexTuple(new double[,]{}, new double[,]{}, new double[,]{}, 0);
		}
	}
}
