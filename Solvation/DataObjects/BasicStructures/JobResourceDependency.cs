namespace Solvation.Domain.BasicStructures
{
	public class JobResourceDependency
	{
		public Job Job { get; set; }
		public Resource Resource { get; set; }
		public double Value { get;set; }
	}
}
