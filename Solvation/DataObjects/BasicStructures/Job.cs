using System.Collections.Generic;
using System.Linq;

namespace DataObjects.BasicStructures
{
	public class Job
	{
		public int Number { get; set; }
		public IEnumerable<int> PrecedingJobs { get; set; }
		public string PrecedingJobsStr { get; set; }
		public double FullWorkVolume { get; set; }
		public double MinimumIntencity { get; set; }
		public double MaximumIntencity { get; set; }

		public Job(int number):this(number, string.Empty, 0, 0, 0)
		{}

		public Job(int number, string precedingJobs, double fullWorkVolume, double minimumIntencity, double maximumIntencity)
		{
			Number = number;
			PrecedingJobsStr = precedingJobs;
			if (!string.IsNullOrEmpty(precedingJobs))
			{
				var pj = Enumerable.Cast<int>(precedingJobs.Split(','));
				PrecedingJobs = pj;
				
			}
			FullWorkVolume = fullWorkVolume;
			MinimumIntencity = minimumIntencity;
			MaximumIntencity = maximumIntencity;
		}
	}
}