using System.Collections.Generic;
using Solvation.Domain.DomainObjects;

namespace Solvation.Domain
{
    public class SchedulingDataContainer
    {
	    private readonly IEnumerable<Resource> resources;
	    private readonly IEnumerable<Job> jobs;
	    private readonly IEnumerable<double[]> dependencies;

	    public IEnumerable<Resource> Resources { get { return resources; }}
		public IEnumerable<Job> Jobs { get { return jobs; } }
		public IEnumerable<double[]> Dependencies { get { return dependencies; } } 

		public SchedulingDataContainer():this(new List<Resource>(), new List<Job>()) {}

		public SchedulingDataContainer(IEnumerable<Resource> res, IEnumerable<Job> jobs):this(res, jobs, null)
		{
		}

		public SchedulingDataContainer(IEnumerable<Resource> res, IEnumerable<Job> jobs, IEnumerable<double[]> dependencies)
		{
			resources = res;
			this.jobs = jobs;
			this.dependencies = dependencies ?? new List<double[]>();
		}
    }
}
