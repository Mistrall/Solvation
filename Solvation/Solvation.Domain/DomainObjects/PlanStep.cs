using System.Collections.Generic;

namespace Solvation.Domain.DomainObjects
{
	public class PlanStep
	{
		public double TimeStart { get; set; }
		public double TimeEnd { get; set; }
		public double TimeDelta { get { return TimeEnd - TimeStart; } }
		public List<RunningJob> ExecutingJobs { get; set; }

		public PlanStep(List<RunningJob> executingJobs, double timeStart, double timeEnd)
		{
			ExecutingJobs = executingJobs;
			TimeStart = timeStart;
			TimeEnd = timeEnd;
		}
	}
}