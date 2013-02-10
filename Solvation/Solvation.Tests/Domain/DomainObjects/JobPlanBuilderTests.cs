using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.Services;

namespace Solvation.Tests.Domain.DomainObjects
{
	[TestFixture]
	public class JobPlanBuilderTests:Assert
	{
		private const double Epsilon = 0.00001;

		[Test]
		public void ShouldBuildPlanForSimpleCase1()
		{
			//Arrange
			var firstJob = new Job(1, 10, null, .1, 2);
			var secondJob = new Job(2, 10, new List<int> { 1 }, .1, 2);
			var thirdJob = new Job(3, 20, new List<int> { 1, 2 }, .1, 2);
			var jobs = new List<Job> { firstJob, secondJob, thirdJob };
			var resource1 = new Resource(1, 20);
			var resources = new List<Resource> { resource1 };
			var dependencies = new List<double[]> { new double[] { 7 }, new double[] { 14 }, new double[] { 10 } };
			var planBuilder = new JobPlanBuilder();
			//Act
			var plan = planBuilder.GetBasePlan(resources, jobs, dependencies);
			//Assert
			AreEqual(plan.Count(), 3);
			AreEqual(plan[0].ExecutingJobs.Count, 1);
			AreEqual(plan[0].ExecutingJobs[0].JobReference.Number, 1);
			True((plan[0].ExecutingJobs[0].Intencity-2)<Epsilon);
			True((plan[0].TimeDelta - 5) < Epsilon);

			AreEqual(plan[1].ExecutingJobs.Count, 1);
			AreEqual(plan[1].ExecutingJobs[0].JobReference.Number, 2);
			True((plan[1].ExecutingJobs[0].Intencity-1.43)<Epsilon);
			True((plan[0].TimeDelta - 7) < Epsilon);

			AreEqual(plan[2].ExecutingJobs.Count, 1);
			AreEqual(plan[2].ExecutingJobs[0].JobReference.Number, 3);
			AreEqual(plan[2].ExecutingJobs[0].Intencity, 2);
			True((plan[0].TimeDelta - 10) < Epsilon);
		}

		[Test]
		public void ShouldCalculateMaxIntensity1()
		{
			//Arrange
			var firstJob = new Job(1, 10, null, .1, 2);
			var resource1 = new Resource(1, 20);
			var dependency = new JobResourceDependency(firstJob, resource1, 7);
			firstJob.ResourceDependencies.Add(dependency);
			var planBuilder = new JobPlanBuilder();
			//Act
			var maxIntensity = planBuilder.CalcMaxPossibleIntensity(firstJob, new[] {resource1});
			//Assert
			True((maxIntensity-2)<Epsilon);
		}

		[Test]
		public void ShouldCalculateMaxIntensity2()
		{
			//Arrange
			var firstJob = new Job(1, 10, null, .1, 2);
			var resource1 = new Resource(1, 20);
			var resource2 = new Resource(1, 15);
			var dependency1 = new JobResourceDependency(firstJob, resource1, 7);
			var dependency2 = new JobResourceDependency(firstJob, resource2, 12);
			firstJob.ResourceDependencies.Add(dependency1);
			firstJob.ResourceDependencies.Add(dependency2);
			var planBuilder = new JobPlanBuilder();
			//Act
			var maxIntensity = planBuilder.CalcMaxPossibleIntensity(firstJob, new[] { resource1, resource2 });
			//Assert
			True((maxIntensity - 1.25) < Epsilon);
		}
	}
}
