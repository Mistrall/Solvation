using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class PlanModel:Observable
	{
		private double scaleDimention;
		private readonly ObservableCollection<RunningJobModel> distinctJobs;
		private readonly ObservableCollection<RunningJobModel> jobs;
		private readonly ObservableCollection<PlanStepModel> steps;

		public ObservableCollection<PlanStepModel> Steps { get { return steps; } }
		public ObservableCollection<RunningJobModel> DistinctJobs { get { return distinctJobs; } }
		public ObservableCollection<RunningJobModel> Jobs { get { return jobs; } }

		public double ScaleDimention
		{
			get { return scaleDimention; }
			set { Set(ref scaleDimention, value, "ScaleDimention"); }
		}

		public PlanModel(IEnumerable<PlanStep> baseStepList)
		{
			steps = new ObservableCollection<PlanStepModel>(baseStepList.Select(bs => new PlanStepModel(bs)).ToList());

			jobs = new ObservableCollection<RunningJobModel>(
				Steps.SelectMany(s => s.ExecutingJobs)
				.Select(j => new RunningJobModel 
				{ 
					Job = j, 
					Name = "Job " + j.JobReference.Number.ToString(CultureInfo.InvariantCulture),
					StartTime = j.StartTime,
					Duration = j.RunTime
				})
				.ToList());

			distinctJobs = new ObservableCollection<RunningJobModel>(jobs.Distinct(new JobModelByNumberComparer()));

			var lastOrDefault = Steps.LastOrDefault();
			if (lastOrDefault != null) scaleDimention = 700 / lastOrDefault.TimeEnd;
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
	}
}
