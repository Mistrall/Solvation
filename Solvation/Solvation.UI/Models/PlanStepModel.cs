using System.Collections.Generic;
using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class PlanStepModel
	{
		public double TimeStart { get; set; }
		public double TimeEnd { get; set; }
		public double TimeDelta { get; private set;}
		public List<RunningJob> ExecutingJobs { get; set; }

		public PlanStepModel(PlanStep step)
		{
			ExecutingJobs = step.ExecutingJobs;
			TimeStart = step.TimeStart;
			TimeEnd = step.TimeEnd;
			TimeDelta = step.TimeDelta;
		}
	}
}
