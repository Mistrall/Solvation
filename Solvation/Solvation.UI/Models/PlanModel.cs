using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class PlanModel:Observable
	{
		private readonly ReadOnlyObservableCollection<RunningJobModel> jobs;
		private readonly ReadOnlyObservableCollection<PlanStepModel> steps;

		public ReadOnlyObservableCollection<PlanStepModel> Steps { get { return steps; } }
		public ReadOnlyObservableCollection<RunningJobModel> Jobs { get { return jobs; } }
		
		public PlanModel(IEnumerable<PlanStep> baseStepList)
		{
			steps = new ReadOnlyObservableCollection<PlanStepModel>(
				new ObservableCollection<PlanStepModel>(baseStepList.Select(bs => new PlanStepModel(bs)).ToList()));

			var jobList = Steps.SelectMany(s => s.ExecutingJobs).Distinct(new JobByNumberComparer())
				.Select(j => new RunningJobModel 
				{ Job = j, 
					Name = "Job " + j.JobReference.Number.ToString(CultureInfo.InvariantCulture),
					Number = j.JobReference.Number
				})
				.ToList();

			jobs = new ReadOnlyObservableCollection<RunningJobModel>(new ObservableCollection<RunningJobModel>(jobList));
		}

		private class JobByNumberComparer : IEqualityComparer<RunningJob> 
		{
			public bool Equals(RunningJob x, RunningJob y)
			{
				if (x == null || y == null) return false;
				return x.JobReference.Number == y.JobReference.Number;
			}

			public int GetHashCode(RunningJob obj)
			{
				return obj.JobReference.Number;
			}
		}
	}
}
