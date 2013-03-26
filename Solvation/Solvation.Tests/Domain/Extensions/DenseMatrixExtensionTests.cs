using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Solvation.Domain.Extensions;

namespace Solvation.Tests.Domain.Extensions
{
	[TestFixture]
	public class DenseMatrixExtensionTests:Assert
	{
		[Test]
		public void ShouldExtractMatrix1()
		{
			//Arrange
			var matrix = new DenseMatrix(new double[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}});
			//Act
			var extracted = matrix.Extract(new[]{1,2}, new[] {0, 2});
			//Assert
			AreEqual(2, extracted.ColumnCount);
			AreEqual(2, extracted.RowCount);
			AreEqual(new []{4.0, 6.0}, extracted.Row(0));
			AreEqual(new[] { 7.0, 9.0 }, extracted.Row(1));
		}

		[Test]
		public void ShouldExtractMatrix2()
		{
			//Arrange
			var matrix = new DenseMatrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });
			//Act
			var extracted = matrix.Extract(new[] { 1 }, new[] { 2 });
			//Assert
			AreEqual(1, extracted.ColumnCount);
			AreEqual(1, extracted.RowCount);
			AreEqual(new[] { 6.0 }, extracted.Row(0));
		}
	}
}
