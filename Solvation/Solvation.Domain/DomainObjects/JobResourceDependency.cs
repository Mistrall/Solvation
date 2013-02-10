namespace Solvation.Domain.DomainObjects
{
	public class JobResourceDependency
	{
		public Job Job { get; set; }
		public Resource Resource { get; set; }
		public double Value { get; set; }

		public JobResourceDependency(Job job, Resource resource, double dependencyValue)
		{
			Job = job;
			Resource = resource;
			Value = dependencyValue;
		}

		#region Equality members

		protected bool Equals(JobResourceDependency other)
		{
			return Equals(Job, other.Job) && Equals(Resource, other.Resource) && Value.Equals(other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((JobResourceDependency) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (Job != null ? Job.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (Resource != null ? Resource.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ Value.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(JobResourceDependency left, JobResourceDependency right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(JobResourceDependency left, JobResourceDependency right)
		{
			return !Equals(left, right);
		}

		#endregion
	}
}
