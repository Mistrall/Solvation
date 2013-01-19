using System.Collections.Generic;

namespace DataObjects
{
    public class SchedulingDataContainer
    {
	    private readonly IEnumerable<Resource> resources;
	    private readonly IEnumerable<Job> jobs;

		public IEnumerable<Resource> Resources { get { return resources; }}
		public IEnumerable<Job> Jobs { get { return jobs; } }

		public SchedulingDataContainer():this(new List<Resource>(), new List<Job>()) {}

		public SchedulingDataContainer(IEnumerable<Resource> res, IEnumerable<Job> jobs)
		{
			this.resources = res;
			this.jobs = jobs;
		}
    }
}
