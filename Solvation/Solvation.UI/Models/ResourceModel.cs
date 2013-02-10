namespace Solvation.UI.Models
{
	public class ResourceModel
	{
		public int Number { get; set; }
		public double Value { get; set; }

		public ResourceModel(int n) : this(n, 0)
		{
		}

		public ResourceModel(int n, double v)
		{
			Number = n;
			Value = v;
		}
	}
}