using System.Collections.Generic;
using System.ComponentModel;
using Solvation.Domain.DomainObjects;

namespace Solvation.Domain.Services
{
	public class JobPlanBuilder
	{
		public IEnumerable<PlanStep> GetBasePlan(BindingList<Resource> resources, BindingList<Job> jobs, BindingList<double[]> dependencyValues)
		{
			var dataContainer = new SchedulingDataContainer(resources, jobs, dependencyValues);

			return new List<PlanStep>();
		}
	}
}