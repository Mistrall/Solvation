using System.Linq;
using NUnit.Framework;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects.Simplex;

namespace Solvation.Tests.AlgorithmHelpers
{
	[TestFixture]
	public class SimplexInputBuilderTests:Assert
	{
		[Test]
		public void ShouldConvertToStandartForm1()
		{
			//Arrange
			const ObjectiveFunctionType type = ObjectiveFunctionType.Min;
			var c = new double[] {-2, 3};
			var A = new double[,]
				{
					{1,1},
					{1, -2},
					{1,0}
				};
			var eqTypes = (new[] {EquationType.Equal, EquationType.LessOrEqual, EquationType.MoreOrEqual}).ToList();
			var b = new double[]{7, 4, 0};
			//Act
			var result = (new SimplexInputBuilder())
				.ConvertToStandartForm(new SimplexTuple(type, A, eqTypes, b, c, 0));
			//Assert
			AreEqual(ObjectiveFunctionType.Max, result.Type);
			AreEqual(new double[]{2, -3}, result.ObjFuncCoeffs.ToArray());
			AreEqual(4, result.EqualityCoeffs.RowCount);
			AreEqual(new double[,]
				{
					{1,1},
					{-1,-1},
					{1, -2},
					{-1,0}
				}, result.EqualityCoeffs.ToArray());
			AreEqual(new[] { EquationType.LessOrEqual, EquationType.LessOrEqual, EquationType.LessOrEqual, EquationType.LessOrEqual }, result.EquationTypes);
			AreEqual(new double[]{7, -7, 4, 0}, result.FreeTerms.ToArray());
		}
	}
}