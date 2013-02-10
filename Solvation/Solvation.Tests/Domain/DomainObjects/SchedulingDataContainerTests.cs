using System.Collections.Generic;
using NUnit.Framework;
using Solvation.Domain.DomainObjects;

namespace Solvation.Tests.Domain.DomainObjects
{
	[TestFixture]
	public class SchedulingDataContainerTests:Assert
	{
		[Test]
		public void ShouldCreateCorrectDataContainer()
		{
			//Arrange
			var firstJob = new Job(1, 100, null, 1, 10);
			var secondJob = new Job(2, 100, new List<int> {1}, 1, 10);
			var thirdJob = new Job(3, 200, new List<int>{1, 2}, 1, 20);
			var jobs = new List<Job> { firstJob, secondJob, thirdJob };
			var resource1 = new Resource(1, 10);
			var resource2 = new Resource(2, 30);
			var resources = new List<Resource> { resource1, resource2 };
			var dependencies = new List<double[]> { new[] {0.7, 1.2} , new[] {1.4, 0.8}, new[] {1.0,2.0} };
			//Act
			var container = new SchedulingDataContainer(resources, jobs, dependencies);
			//Assert
			AreEqual(container.Jobs, jobs);
			AreEqual(container.Resources, resources);

			AreEqual(container.Jobs[0].Number, 1);
			AreEqual(container.Jobs[0].FullWorkVolume, 100);
			AreEqual(container.Jobs[0].MinimumIntencity, 1);
			AreEqual(container.Jobs[0].MaximumIntencity, 10);
			AreEqual(container.Jobs[0].ResourceDependencies.Count, 2);
			AreEqual(container.Jobs[0].PrecedingJobs.Count, 0);
			AreEqual(container.Jobs[0].NumberOfDependants, 2);
			AreEqual(container.Jobs[0].ResourceDependencies[0].Job, firstJob);
			AreEqual(container.Jobs[0].ResourceDependencies[0].Resource, resource1);
			AreEqual(container.Jobs[0].ResourceDependencies[0].Value, 0.7);
			AreEqual(container.Jobs[0].ResourceDependencies[1].Job, firstJob);
			AreEqual(container.Jobs[0].ResourceDependencies[1].Resource, resource2);
			AreEqual(container.Jobs[0].ResourceDependencies[1].Value, 1.2);

			AreEqual(container.Jobs[1].Number, 2);
			AreEqual(container.Jobs[1].PrecedingJobs.Count, 1);
			AreEqual(container.Jobs[1].PrecedingJobs[0], firstJob);
			AreEqual(container.Jobs[1].NumberOfDependants, 1);
			AreEqual(container.Jobs[1].ResourceDependencies.Count, 2);
			AreEqual(container.Jobs[1].ResourceDependencies[0].Job, secondJob);
			AreEqual(container.Jobs[1].ResourceDependencies[0].Resource, resource1);
			AreEqual(container.Jobs[1].ResourceDependencies[0].Value, 1.4);
			AreEqual(container.Jobs[1].ResourceDependencies[1].Job, secondJob);
			AreEqual(container.Jobs[1].ResourceDependencies[1].Resource, resource2);
			AreEqual(container.Jobs[1].ResourceDependencies[1].Value, 0.8);

			AreEqual(container.Jobs[2].Number, 3);
			AreEqual(container.Jobs[2].PrecedingJobs.Count, 2);
			AreEqual(container.Jobs[2].PrecedingJobs[0], firstJob);
			AreEqual(container.Jobs[2].PrecedingJobs[1], secondJob);
			AreEqual(container.Jobs[2].ResourceDependencies.Count, 2);
			AreEqual(container.Jobs[2].ResourceDependencies[0].Job, thirdJob);
			AreEqual(container.Jobs[2].ResourceDependencies[0].Resource, resource1);
			AreEqual(container.Jobs[2].ResourceDependencies[0].Value, 1.0);
			AreEqual(container.Jobs[2].ResourceDependencies[1].Job, thirdJob);
			AreEqual(container.Jobs[2].ResourceDependencies[1].Resource, resource2);
			AreEqual(container.Jobs[2].ResourceDependencies[1].Value, 2.0);
		}
	}
}
