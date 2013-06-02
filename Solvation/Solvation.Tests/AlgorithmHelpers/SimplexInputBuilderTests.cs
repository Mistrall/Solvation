using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.DomainObjects.Simplex;
using Solvation.Domain.Services;

namespace Solvation.Tests.AlgorithmHelpers
{
	[TestFixture]
	public class SimplexInputBuilderTests : Assert
	{
		[Test]
		public void ShouldConvertToStandartForm1()
		{
			//Arrange
			const ObjectiveFunctionType type = ObjectiveFunctionType.Min;
			var c = new double[] {-2, 3};
			var A = new double[,]
				{
					{1, 1},
					{1, -2},
					{1, 0}
				};
			var eqTypes = (new[] {EquationType.Equal, EquationType.LessOrEqual, EquationType.MoreOrEqual}).ToList();
			var b = new double[] {7, 4, 0};
			//Act
			var result = (new SimplexInputBuilder())
				.ConvertToStandartForm(new SimplexTuple(type, A, eqTypes, b, c, 0));
			//Assert
			AreEqual(ObjectiveFunctionType.Max, result.Type);
			AreEqual(new double[] {2, -3}, result.ObjFuncCoeffs.ToArray());
			AreEqual(4, result.EqualityCoeffs.RowCount);
			AreEqual(new double[,]
				{
					{1, 1},
					{-1, -1},
					{1, -2},
					{-1, 0}
				}, result.EqualityCoeffs.ToArray());
			AreEqual(
				new[] {EquationType.LessOrEqual, EquationType.LessOrEqual, EquationType.LessOrEqual, EquationType.LessOrEqual},
				result.EquationTypes);
			AreEqual(new double[] {7, -7, 4, 0}, result.FreeTerms.ToArray());
		}

		[Test]
		public void ShouldCreateLinearProgramFromBasePlan1()
		{
			//Arrange
			var job1 = new Job(1, 100, null, 1, 10);
			var job2 = new Job(2, 50, new[] {1}, 1, 10);
			var job3 = new Job(3, 50, null, 1, 10);
			var jobArr = new List<Job> {job1, job2, job3};

			var resourceArray = new List<Resource> {new Resource(1, 20), new Resource(2, 50)};

			var dependencies = new List<double[]>
				{
					new double[] {1, 2},
					new double[] {2, 2},
					new double[] {0.7, 3.5}
				};

			var baseStepList = (new JobPlanBuilder()).GetBasePlan(resourceArray, jobArr, dependencies);
			//Act
			var result = (new SimplexInputBuilder()).BuildFromBasePlan(baseStepList, jobArr, resourceArray, dependencies);
			//Assert
			NotNull(result);
			AreEqual(new double[] {1, 1, 1, 0, 0, 0, 0}, result.ObjFuncCoeffs.ToArray());
			AreEqual(ObjectiveFunctionType.Min, result.Type);
			AreEqual(27, result.EqualityCoeffs.RowCount);
			//Step1
			//intencity
			AreEqual(new double[] {1, 0, 0, -1, 0, 0, 0}, result.EqualityCoeffs.Row(7).ToArray());
			AreEqual(new double[] {-10, 0, 0, 1, 0, 0, 0}, result.EqualityCoeffs.Row(8).ToArray());
			AreEqual(new double[] {1, 0, 0, 0, -1, 0, 0}, result.EqualityCoeffs.Row(9).ToArray());
			AreEqual(new double[] {-10, 0, 0, 0, 1, 0, 0}, result.EqualityCoeffs.Row(10).ToArray());
			//resources
			AreEqual(new double[] { -20, 0, 0, 1, 0.7, 0, 0 }, result.EqualityCoeffs.Row(11).ToArray());
			AreEqual(new double[] { -50, 0, 0, 2, 3.5, 0, 0 }, result.EqualityCoeffs.Row(12).ToArray());
			//Step2
			//intencity
			AreEqual(new double[] {0, 1, 0, 0, 0, -1, 0}, result.EqualityCoeffs.Row(13).ToArray());
			AreEqual(new double[] {0, -10, 0, 0, 0, 1, 0}, result.EqualityCoeffs.Row(14).ToArray());
			//resources
			AreEqual(new double[] { 0, -20, 0, 0, 0, 1, 0 }, result.EqualityCoeffs.Row(15).ToArray());
			AreEqual(new double[] { 0, -50, 0, 0, 0, 2, 0 }, result.EqualityCoeffs.Row(16).ToArray());
			//Step3
			//intencity
			AreEqual(new double[] {0, 0, 1, 0, 0, 0, -1}, result.EqualityCoeffs.Row(17).ToArray());
			AreEqual(new double[] {0, 0, -10, 0, 0, 0, 1}, result.EqualityCoeffs.Row(18).ToArray());
			//resources
			AreEqual(new double[] { 0, 0, -20, 0, 0, 0, 2 }, result.EqualityCoeffs.Row(19).ToArray());
			AreEqual(new double[] { 0, 0, -50, 0, 0, 0, 2 }, result.EqualityCoeffs.Row(20).ToArray());

			//Full work volumes
			AreEqual(new double[] { 0, 0, 0, 1, 0, 1, 0 }, result.EqualityCoeffs.Row(21).ToArray());
			AreEqual(100, result.FreeTerms.ToArray()[21]);
			AreEqual(new double[] { 0, 0, 0, -1, 0, -1, 0 }, result.EqualityCoeffs.Row(22).ToArray());
			AreEqual(-100, result.FreeTerms.ToArray()[22]);

			AreEqual(new double[] { 0, 0, 0, 0, 0, 0, 1 }, result.EqualityCoeffs.Row(23).ToArray());
			AreEqual(50, result.FreeTerms.ToArray()[23]);
			AreEqual(new double[] { 0, 0, 0, 0, 0, 0, -1 }, result.EqualityCoeffs.Row(24).ToArray());
			AreEqual(-50, result.FreeTerms.ToArray()[24]);

			AreEqual(new double[] { 0, 0, 0, 0, 1, 0, 0 }, result.EqualityCoeffs.Row(25).ToArray());
			AreEqual(50, result.FreeTerms.ToArray()[25]);
			AreEqual(new double[] { 0, 0, 0, 0, -1, 0, 0 }, result.EqualityCoeffs.Row(26).ToArray());
			AreEqual(-50, result.FreeTerms.ToArray()[26]);
		}
	}
}