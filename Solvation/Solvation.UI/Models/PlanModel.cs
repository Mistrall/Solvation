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
		private readonly ObservableCollection<RunningJobModel> distinctJobs;
		private readonly ObservableCollection<RunningJobModel> jobs;
		private readonly ObservableCollection<PlanStepModel> steps;
		private readonly ObservableCollection<JobDependencyModel> dependencies;

		public ObservableCollection<PlanStepModel> Steps { get { return steps; } }
		public ObservableCollection<RunningJobModel> DistinctJobs { get { return distinctJobs; } }
		public ObservableCollection<RunningJobModel> Jobs { get { return jobs; } }
		public ObservableCollection<JobDependencyModel> Dependencies { get { return dependencies; } }

		public ReadOnlyObservableCollection<object> JobsWithDepedencies { get; private set; }

		public double ScaleDimension
		{
			get { return scaleDimension; }
			set { Set(ref scaleDimension, value, "ScaleDimension"); }
		}

		public PlanModel(IEnumerable<PlanStep> baseStepList)
		{
			steps = new ObservableCollection<PlanStepModel>(baseStepList.Select(bs => new PlanStepModel(bs)).ToList());

			var jobList = new List<RunningJobModel>();
			var distinctJobList = new List<RunningJobModel>();
			var dependencyList = new List<JobDependencyModel>();

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
					var dependency = new JobDependencyModel
						{
							Dependant = runningJob.Job,
							DependantOn = distinctJobList.Where(j => j.Number == precedingJob.Number).Select(j => j.Job).SingleOrDefault()
						};

					if (!dependencyList.Contains(dependency, new JobDependencyComparer()))
						dependencyList.Add(dependency);
				}
			}


			distinctJobs = new ObservableCollection<RunningJobModel>(distinctJobList);
			jobs = new ObservableCollection<RunningJobModel>(jobList);
			dependencies = new ObservableCollection<JobDependencyModel>(dependencyList);

			var obs = new List<object>();
			obs.AddRange(jobs);
			obs.AddRange(dependencies);

			JobsWithDepedencies = new ReadOnlyObservableCollection<object>(new ObservableCollection<object>(obs));

			var lastOrDefault = Steps.LastOrDefault();
			if (lastOrDefault != null) scaleDimension = 700 / lastOrDefault.TimeEnd;
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
