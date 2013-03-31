using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.DomainObjects.Simplex
{
	public class SimplexResult
	{
		public double OptimalValue { get; set; }
		public Vector OptimalVector { get; set; }
	}
}
