using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Solvation.Domain.DomainObjects
{
    public class SchedulingDataContainer
    {
	    private readonly List<Resource> resources;
	    private readonly Dictionary<String, Job> jobs;
	    private readonly List<double[]> dependencies;

	    public List<Resource> Resources { get { return resources; }}
		public List<Job> Jobs { get { return jobs.Values.ToList(); } }
		public List<double[]> Dependencies { get { return dependencies; } } 

		public SchedulingDataContainer():this(new List<Resource>(), new List<Job>()) {}

		public SchedulingDataContainer(IEnumerable<Resource> res, IEnumerable<Job> jobs):this(res, jobs, null)
		{
		}

		public SchedulingDataContainer(IEnumerable<Resource> res, IEnumerable<Job> jobs, IEnumerable<double[]> dependencies)
		{
			var resArr = res as Resource[] ?? res.ToArray();
			resources = res.ToList();
			var jobsArr = jobs as Job[] ?? jobs.ToArray();
			var depArr = dependencies as double[][] ?? dependencies.ToArray();
			
			//Add all jobs to hash map
			var jobsDict = jobsArr.ToDictionary(job => job.Number.ToString(CultureInfo.InvariantCulture));
			//populate dependencies and preceding jobs
			for (int j = 0; j < jobsArr.Length;j++ )
			{
				for (int r=0;r<resArr.Length;r++)
				{
					var dep = new JobResourceDependency(jobsArr[j], resArr[r], depArr[j][r]);
					jobsDict[jobsArr[j].Number.ToString(CultureInfo.InvariantCulture)].ResourceDependencies.Add(dep);
				}
				if (jobsArr[j].PrecedingJobNums.Count > 0)
				{
					foreach (var pjob in jobsArr[j].PrecedingJobNums)
					{
						jobsDict[jobsArr[j].Number.ToString(CultureInfo.InvariantCulture)]
							.PrecedingJobs.Add(jobsDict[pjob.ToString(CultureInfo.InvariantCulture)]);

						jobsDict[pjob.ToString(CultureInfo.InvariantCulture)].
							DependantJobs.Add(jobsDict[jobsArr[j].Number.ToString(CultureInfo.InvariantCulture)]);
					}
				}
			}

			this.jobs = jobsDict;
			this.dependencies = depArr.ToList();
		}
    }
}
