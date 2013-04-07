using System;
using System.Collections.Generic;
using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class PlanStepModel : Observable
	{
		public int Number { get; private set; }
		public double TimeStart { get; set; }
		public double TimeEnd { get; set; }
		public double TimeDelta { get; private set;}
		public List<RunningJob> ExecutingJobs { get; set; }

		public string StepDescription { get; private set; }

		public PlanStepModel(PlanStep step)
		{
			ExecutingJobs = step.ExecutingJobs;
			TimeStart = step.TimeStart;
			TimeEnd = step.TimeEnd;
			TimeDelta = step.TimeDelta;
			Number = step.Number;
			StepDescription = string.Format("| Step {0}, Duration {1}", Number, String.Format("{0:0.##}", TimeDelta));
		}
	}
}
