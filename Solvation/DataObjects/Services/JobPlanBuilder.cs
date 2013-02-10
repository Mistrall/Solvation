using System.Collections.Generic;
using System.ComponentModel;
using Solvation.Domain.DomainObjects;

namespace Solvation.Domain.Services
{
	public class JobPlanBuilder
	{
		public IEnumerable<PlanStep> GetBasePlan(IEnumerable<Resource> resources, IEnumerable<Job> jobs, IEnumerable<double[]> dependencyValues)
		{
			var dataContainer = new SchedulingDataContainer(resources, jobs, dependencyValues);

			return new List<PlanStep>();
		}
	}
}