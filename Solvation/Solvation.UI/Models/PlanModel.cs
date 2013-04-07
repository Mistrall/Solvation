using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class PlanModel : Observable
	{
		private double scaleDimension;
		private const double EPSILON = 0.000001;
		private readonly ObservableCollection<RunningJobModel> distinctJobs;
		private readonly ObservableCollection<RunningJobModel> jobs;
		private readonly ObservableCollection<PlanStepModel> steps;
		private readonly ObservableCollection<JobDependencyModel> dependencies;

		public ObservableCollection<PlanStepModel> Steps
		{
			get { return steps; }
		}

		public ObservableCollection<RunningJobModel> DistinctJobs
		{
			get { return distinctJobs; }
		}

		public ObservableCollection<RunningJobModel> Jobs
		{
			get { return jobs; }
		}

		public ObservableCollection<JobDependencyModel> Dependencies
		{
			get { return dependencies; }
		}

		public ReadOnlyObservableCollection<object> JobsWithDepedencies { get; private set; }

		public double ScaleDimension
		{
			get { return scaleDimension; }
			set { Set(ref scaleDimension, value, "ScaleDimension"); }
		}

		public IEnumerable<PlanStep> Plan { get; private set; }

		public PlanModel(IEnumerable<PlanStep> baseStepList)
		{
			Plan = baseStepList;
			steps = new ObservableCollection<PlanStepModel>(Plan.Select(bs => new PlanStepModel(bs)).ToList());

			var jobList = new List<RunningJobModel>();
			var distinctJobList = new List<RunningJobModel>();
			
			var dependencyHashTable = new Dictionary<int, JobDependencyModel>();

			jobList =
				Steps.SelectMany(s => s.ExecutingJobs)
				     .Select(j => new RunningJobModel
					     {
						     Job = j,
						     Name = "Job " + j.JobReference.Number.ToString(CultureInfo.InvariantCulture),
						     StartTime = j.StartTime,
						     Duration = j.RunTime
					     })
				     .ToList();


			distinctJobList = jobList.Distinct(new JobModelByNumberComparer()).ToList();

			foreach (var runningJob in distinctJobList)
			{
				foreach (var precedingJob in runningJob.Job.JobReference.PrecedingJobs)
				{
					var allDependantOn = jobList.Where(j => j.Number == precedingJob.Number);
					var maxTime = allDependantOn.Max(j1 => j1.EndTime);
					var lastDependantOn = allDependantOn.SingleOrDefault(j => Math.Abs(j.EndTime - maxTime) < EPSILON);

					if (lastDependantOn == null) continue;

					var dependency = new JobDependencyModel
						{
							Dependant = runningJob.Job,
							DependantOn = lastDependantOn.Job
						};

					if (dependencyHashTable.ContainsKey(dependency.DependencyKey))
					{
						dependencyHashTable[dependency.DependencyKey] = dependency;
					}
					else dependencyHashTable.Add(dependency.DependencyKey, dependency);
				}
			}

			distinctJobs = new ObservableCollection<RunningJobModel>(distinctJobList);
			jobs = new ObservableCollection<RunningJobModel>(jobList);
			dependencies = new ObservableCollection<JobDependencyModel>(dependencyHashTable.Values.ToList());

			var obs = new List<object>();
			obs.AddRange(jobs);
			obs.AddRange(dependencies);
			obs.AddRange(steps);

			JobsWithDepedencies = new ReadOnlyObservableCollection<object>(new ObservableCollection<object>(obs));

			var lastOrDefault = Steps.LastOrDefault();
			if (lastOrDefault != null) scaleDimension = 700/lastOrDefault.TimeEnd;
		}

		private class JobModelByNumberComparer : IEqualityComparer<RunningJobModel>
		{
			public bool Equals(RunningJobModel x, RunningJobModel y)
			{
				if (x == null || y == null) return false;
				return x.Number == y.Number;
			}

			public int GetHashCode(RunningJobModel obj)
			{
				return obj.Number;
			}
		}

		private class JobDependencyComparer : IEqualityComparer<JobDependencyModel>
		{
			public bool Equals(JobDependencyModel x, JobDependencyModel y)
			{
				if (x == null || y == null) return false;
				return (x.Dependant.JobReference.Number == y.Dependant.JobReference.Number)
				       && (x.DependantOn.JobReference.Number == y.DependantOn.JobReference.Number);
			}

			public int GetHashCode(JobDependencyModel obj)
			{
				return obj.Dependant.JobReference.Number ^ obj.DependantOn.JobReference.Number;
			}
		}
	}
}