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
			get { return number; }
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
	}
}
