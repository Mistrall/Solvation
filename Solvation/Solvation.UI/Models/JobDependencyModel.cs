using Solvation.Domain.DomainObjects;

namespace Solvation.UI.Models
{
	public class JobDependencyModel:Observable
	{
		private RunningJob dependant;
		private RunningJob dependantOn;

		public RunningJob Dependant
		{
			get { return dependant; }
			set { Set(ref dependant, value, "Dependant"); }
		}

		public RunningJob DependantOn
		{
			get { return dependantOn; }
			set { Set(ref dependantOn, value, "DependantOn"); }
		}

		public int RowDifference
		{
			get { return Dependant.JobReference.Number - DependantOn.JobReference.Number; }
		}

		public double Distance
		{
			get { return Dependant.StartTime - (DependantOn.StartTime + DependantOn.RunTime); }
		}
	}
}
