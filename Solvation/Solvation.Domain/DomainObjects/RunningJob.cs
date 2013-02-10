namespace Solvation.Domain.DomainObjects
{
	public class RunningJob
	{
		public Job JobReference { get; set; }
		public double Intencity { get; set; }
		public double RunTime { get; set; }

		public RunningJob(Job jobReference, double intencity, double runTime)
		{
			JobReference = jobReference;
			Intencity = intencity;
			RunTime = runTime;
		}
	}
}
