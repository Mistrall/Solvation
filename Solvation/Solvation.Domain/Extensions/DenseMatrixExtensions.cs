using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.Extensions
{
	public static class DenseMatrixExtensions
	{
		public static DenseMatrix Extract(this DenseMatrix matrix, int[] rows, int[] columns)
		{
			var extractedMatrix = new DenseMatrix(rows.Length, columns.Length);

			for (int c = 0; c < columns.Length; c++)
			{
				for (int r = 0; r < rows.Length; r++)
				{
					extractedMatrix[r, c] = matrix[rows[r], columns[c]];
				}
			}

			return extractedMatrix;
		}
	}
}
