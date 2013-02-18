using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class RunningJobModel:Observable
	{
		private string name;
		private RunningJob job;
		private int number;

		public int Number
		{
			get
			{
				if (number == 0) number = job.JobReference.Number;
				return number;
			}
			set { number = value; }
		}

		public RunningJob Job
		{
			get { return job; }
			set { job = value; }
		}

		public string Name
		{
			get { return name; }
			set { Set(ref name, value, "Name"); }
		}

		public double StartTime { get; set; }
		public double Duration { get; set; }
	}
}
