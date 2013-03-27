using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Solvation.Domain.Services;

namespace Solvation.Tests.Services
{
	[TestFixture]
	public class SimplexSolverTests:Assert
	{
		[Test]
		public void ShouldSolveSimpleCase1()
		{
			//Arrange
			var A = new DenseMatrix(new double[,]
				{
					{1, 0, 0, 0},
					{20, 1, 0, 0}, {200, 20, 1, 0}, {2000, 200, 20, 1}, {-1, 0, 0, 0}, {0, -1, 0, 0},
					{0, 0, -1, 0}, {0, 0, 0, -1}
				});
			var b = new DenseVector(new double[] {1, 100, 10000, 1000000, 0, 0, 0, 0});

			var c = new DenseVector(new double[] {1000, 100, 10, 1});
			var B = Enumerable.Range(4, 4).ToArray();
			//Act
			var result = (new SimplexSolver()).Solve(A, b, c, B);
			//Assert
			AreEqual(4, result.Count);
			AreEqual(new double[]{0, 0, 0, 0}, result.ToArray());
		}
	}
}