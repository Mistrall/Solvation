using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.Extensions
{
	public static class DenseVectorExtenstions
	{
		public static DenseVector Extract(this DenseVector vector, int[] rows)
		{
			var extractedVector = new DenseVector(rows.Length);

			for (int i = 0; i < rows.Length; i++)
			{
				extractedVector[i] = vector[rows[i]];
			}

			return extractedVector;
		}
	}
}
