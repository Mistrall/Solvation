using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.Extensions
{
	public static class SparseVectorExtensions
	{
		public static SparseVector Extract(this SparseVector vector, int[] rows)
		{
			var extractedVector = new SparseVector(rows.Length);

			for (int i = 0; i < rows.Length; i++)
			{
				extractedVector[i] = vector[rows[i]];
			}

			return extractedVector;
		}
	}
}
