namespace DataObjects.BasicStructures
{
	public class Resource
	{
		public int Number { get; set; }
		public double Value { get; set; }

		public Resource(int n) : this(n, 0)
		{
		}

		public Resource(int n, double v)
		{
			Number = n;
			Value = v;
		}
	}
}