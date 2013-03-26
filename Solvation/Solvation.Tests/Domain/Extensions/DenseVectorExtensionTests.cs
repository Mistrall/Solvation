using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Solvation.Domain.Extensions;

namespace Solvation.Tests.Domain.Extensions
{
	[TestFixture]
	public class DenseVectorExtensionTests:Assert
	{
		[Test]
		public void ShouldExtractVector()
		{
			//Arrange
			var vector = new DenseVector(new[]{1, 2, 3, 4, 5 ,6 ,7.0});
			//Act
			var extracted = vector.Extract(new[] {0, 2, 5});
			//Assert
			AreEqual(3, extracted.Count);
			AreEqual(new []{1.0, 3.0, 6.0}, extracted.ToArray());
		}
	}
}
