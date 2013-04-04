using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.Extensions;
using Solvation.Domain.Services;

namespace Solvation.Tests.Services
{
	[TestFixture]
	public class JobPlanBuilderTests:Assert
	{
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
			True(plan[0].ExecutingJobs[0].Intencity.FloatEquals(2));
			True(plan[0].TimeDelta.FloatEquals(5));

			AreEqual(plan[1].ExecutingJobs.Count, 1);
			AreEqual(plan[1].ExecutingJobs[0].JobReference.Number, 2);
			True(plan[1].ExecutingJobs[0].Intencity.FloatEquals(1.42857142857));
			True(plan[1].TimeDelta.FloatEquals(7));

			AreEqual(plan[2].ExecutingJobs.Count, 1);
			AreEqual(plan[2].ExecutingJobs[0].JobReference.Number, 3);
			AreEqual(plan[2].ExecutingJobs[0].Intencity, 2);
			True(plan[2].TimeDelta.FloatEquals(10));
		}

		[Test]
		public void ShouldBuildPlanForSimpleCase2()
		{
			//Arrange
			var firstJob = new Job(1, 10, null, .1, 2);
			var secondJob = new Job(2, 10, new List<int> { 1 }, .1, 2);
			var thirdJob = new Job(3, 20, new List<int> { 1, 2 }, .1, 2);
			var fourthJob = new Job(4, 20, null, .1, 2);
			var jobs = new List<Job> { firstJob, secondJob, thirdJob, fourthJob };
			var resource1 = new Resource(1, 20);
			var resources = new List<Resource> { resource1 };
			var dependencies = new List<double[]> { new double[] { 7 }, new double[] { 14 }, new double[] { 10 }, new double[] { 6 } };
			var planBuilder = new JobPlanBuilder();
			//Act
			var plan = planBuilder.GetBasePlan(resources, jobs, dependencies);
			//Assert
			AreEqual(plan.Count(), 4);
			AreEqual(plan[0].ExecutingJobs.Count, 2);
			AreEqual(plan[0].ExecutingJobs[0].JobReference.Number, 1);
			AreEqual(plan[0].ExecutingJobs[1].JobReference.Number, 4);
			True(plan[0].ExecutingJobs[0].Intencity.FloatEquals(2));
			True(plan[0].ExecutingJobs[1].Intencity.FloatEquals(1));
			True(plan[0].ExecutingJobs[1].JobReference.RemainingVolume.FloatEquals(0.0));
			True(plan[0].TimeDelta.FloatEquals(5));

			AreEqual(plan[1].ExecutingJobs.Count, 1);
			AreEqual(plan[1].ExecutingJobs[0].JobReference.Number, 2);
			True(plan[1].ExecutingJobs[0].Intencity.FloatEquals(1.42857142857));
			True(plan[1].TimeDelta.FloatEquals(7));

			AreEqual(plan[2].ExecutingJobs.Count, 1);
			AreEqual(plan[2].ExecutingJobs[0].JobReference.Number, 3);
			AreEqual(plan[2].ExecutingJobs[0].Intencity, 2);
			True(plan[2].TimeDelta.FloatEquals(10));

			AreEqual(plan[3].ExecutingJobs.Count, 1);
			AreEqual(plan[3].ExecutingJobs[0].JobReference.Number, 4);
			AreEqual(plan[3].ExecutingJobs[0].Intencity, 2);
			True(plan[3].TimeDelta.FloatEquals(7.5));
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
			True(maxIntensity.FloatEquals(2));
		}

		[Test]
		public void ShouldCalculateMaxIntensity2()
		{
			//Arrange
			var firstJob = new Job(1, 10, null, .1, 2);
			var resource1 = new Resource(1, 20);
			var resource2 = new Resource(2, 15);
			var dependency1 = new JobResourceDependency(firstJob, resource1, 7);
			var dependency2 = new JobResourceDependency(firstJob, resource2, 12);
			firstJob.ResourceDependencies.Add(dependency1);
			firstJob.ResourceDependencies.Add(dependency2);
			var planBuilder = new JobPlanBuilder();
			//Act
			var maxIntensity = planBuilder.CalcMaxPossibleIntensity(firstJob, new[] { resource1, resource2 });
			//Assert
			True(maxIntensity.FloatEquals(1.25));
		}
	}
}
