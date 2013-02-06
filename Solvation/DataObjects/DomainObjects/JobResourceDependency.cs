namespace Solvation.Domain.DomainObjects
{
	public class JobResourceDependency
	{
		public Job Job { get; set; }
		public Resource Resource { get; set; }
		public double Value { get;set; }
	}
}
