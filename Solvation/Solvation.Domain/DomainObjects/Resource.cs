namespace Solvation.Domain.DomainObjects
{
	public class Resource
	{
		public int Number { get; set; }
		public double Value { get; set; }

		public Resource(int number, double value)
		{
			Number = number;
			Value = value;
		}

		#region Equality members

		protected bool Equals(Resource other)
		{
			return Number == other.Number && Value.Equals(other.Value);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Resource) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Number*397) ^ Value.GetHashCode();
			}
		}

		public static bool operator ==(Resource left, Resource right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Resource left, Resource right)
		{
			return !Equals(left, right);
		}

		#endregion

		public Resource DeepCopy()
		{
			return new Resource(Number, Value);
		}
	}
}
