using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.Extensions
{
	public static class SparseMatrixExtensions
	{
		public static SparseMatrix Extract(this SparseMatrix matrix, int[] rows, int[] columns)
		{
			var extractedMatrix = new SparseMatrix(rows.Length, columns.Length);

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
