using System.Globalization;

namespace Solvation.Domain.DomainObjects
{
	public class RunningJob
	{
		public Job JobReference { get; set; }
		public double Intencity { get; set; }
		public double RunTime { get; set; }
		public double StartTime { get; set; }

		public double EndTime { get { return StartTime + RunTime; } }

		public string JobNumber { get { return JobReference.Number.ToString(CultureInfo.InvariantCulture); } }

		public RunningJob(Job jobReference, double intencity, double runTime, double startTime)
		{
			JobReference = jobReference;
			Intencity = intencity;
			RunTime = runTime;
			StartTime = startTime;
		}
	}
}
