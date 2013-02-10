using System.Collections.Generic;
using System.Linq;

namespace Solvation.Domain.DomainObjects
{
	public class Job
	{
		private readonly List<int> precedingJobNums;

		public int Number { get; set; }
		public List<Job> PrecedingJobs { get; set; }
		public List<JobResourceDependency> Dependencies { get; set; }
		public double FullWorkVolume { get; set; }
		public double MinimumIntencity { get; set; }
		public double MaximumIntencity { get; set; }

		public List<int> PrecedingJobNums
		{
			get { return precedingJobNums; }
		}

		public Job(int number, double fullWorkVolume, IEnumerable<int> precedingJobNums, double minimumIntencity, double maximumIntencity)
		{
			Number = number;
			this.precedingJobNums = precedingJobNums != null ? precedingJobNums.ToList() : new List<int>();
			
			FullWorkVolume = fullWorkVolume;
			MinimumIntencity = minimumIntencity;
			MaximumIntencity = maximumIntencity;

			Dependencies = new List<JobResourceDependency>();
			PrecedingJobs = new List<Job>();
		}

		#region Equality members
		protected bool Equals(Job other)
		{
			return Number == other.Number;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Job) obj);
		}

		public override int GetHashCode()
		{
			return Number;
		}

		public static bool operator ==(Job left, Job right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Job left, Job right)
		{
			return !Equals(left, right);
		}
		#endregion
	}
}
