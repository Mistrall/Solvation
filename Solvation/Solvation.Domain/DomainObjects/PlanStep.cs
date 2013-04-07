using System.Collections.Generic;

namespace Solvation.Domain.DomainObjects
{
	public class PlanStep
	{
		public int Number { get; private set; }
		public double TimeStart { get; set; }
		public double TimeEnd { get; set; }
		public double TimeDelta { get; private set;}
		public List<RunningJob> ExecutingJobs { get; set; }

		public PlanStep(List<RunningJob> executingJobs, double timeStart, double timeEnd, int stepNumber)
		{
			ExecutingJobs = executingJobs;
			TimeStart = timeStart;
			TimeEnd = timeEnd;
			TimeDelta = timeEnd - timeStart;
			Number = stepNumber;
		}
	}
}