using System.Collections.Generic;
using System.Linq;

namespace Solvation.Domain.DomainObjects
{
	public class Job
	{
		private readonly List<int> precedingJobNums;

		public int Number { get; set; }
		//TODO: Add validation for duplicates and cycles
		public List<Job> PrecedingJobs { get; set; }
		//TODO: Add validation for duplicates and cycles
		public List<Job> DependantJobs { get; set; }
		public List<JobResourceDependency> ResourceDependencies { get; set; }
		public double FullWorkVolume { get; set; }
		public double RemainingVolume { get; set; }
		public double MinimumIntensity { get; set; }
		public double MaximumIntensity { get; set; }
		public int NumberOfDependants { get { return DependantJobs.Count; } }
		public JobState State { get; set; }

		public List<int> PrecedingJobNums { get { return precedingJobNums; } }

		public Job(int number, double fullWorkVolume, IEnumerable<int> precedingJobNums, double minimumIntencity, double maximumIntensity)
		{
			Number = number;
			this.precedingJobNums = precedingJobNums != null ? precedingJobNums.ToList() : new List<int>();
			
			FullWorkVolume = fullWorkVolume;
			RemainingVolume = fullWorkVolume;
			MinimumIntensity = minimumIntencity;
			MaximumIntensity = maximumIntensity;
			State = JobState.NotStarted;

			ResourceDependencies = new List<JobResourceDependency>();
			PrecedingJobs = new List<Job>();
			DependantJobs = new List<Job>();
		}

		public bool CanStart()
		{
			return PrecedingJobs.All(j => j.State == JobState.Finished);
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

		//TODO: This recursion can cause stack overflow if we will have incorrect configuration, check it. 
		//TODO: Also make it lazy field of job instance, not static, assign after we compute it one time. Will reduce number of recursive calls significantly
		public static double GetFullVolumeWithDependantJobs(Job job)
		{
			var dependantVolume = 0.0;
			if (job.NumberOfDependants != 0) 
			{
				dependantVolume += job.DependantJobs.Sum(depJob => GetFullVolumeWithDependantJobs(depJob));
			}
			return job.FullWorkVolume+dependantVolume;
		}
	}

	public enum JobState
	{
		NotStarted = 0,
		Started = 1,
		Finished = 2
	}
}
