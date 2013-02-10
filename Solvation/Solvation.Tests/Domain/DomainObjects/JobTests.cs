using NUnit.Framework;
using Solvation.Domain.DomainObjects;

namespace Solvation.Tests.Domain.DomainObjects
{
	[TestFixture]
	public class JobTests:Assert
	{
		[Test]
		public void ShouldCountFullVolumeWithDependant()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 10, null, 1, 10);
			var job3 = new Job(3, 10, null, 1, 10);
			var job4 = new Job(4, 10, null, 1, 10);
			job1.DependantJobs.Add(job2);
			job1.DependantJobs.Add(job3);
			job4.DependantJobs.Add(job3);
			//Act
			var fullVolumeWithDependant1 = Job.GetFullVolumeWithDependantJobs(job1);
			var fullVolumeWithDependant2 = Job.GetFullVolumeWithDependantJobs(job2);
			var fullVolumeWithDependant3 = Job.GetFullVolumeWithDependantJobs(job3);
			var fullVolumeWithDependant4 = Job.GetFullVolumeWithDependantJobs(job4);
			//Assert
			AreEqual(fullVolumeWithDependant1, 30);
			AreEqual(fullVolumeWithDependant2, 10);
			AreEqual(fullVolumeWithDependant3, 10);
			AreEqual(fullVolumeWithDependant4, 20);
		}

		[Test]
		public void ShouldBeAbleToStartJob()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 10, null, 1, 10);
			var job3 = new Job(3, 10, null, 1, 10);
			var job4 = new Job(4, 10, null, 1, 10);
			job2.PrecedingJobs.Add(job1);
			job3.PrecedingJobs.Add(job2);
			job3.PrecedingJobs.Add(job4);
			//Act
			var result1 = job1.CanStart();
			var result2 = job2.CanStart();
			var result3 = job3.CanStart();
			var result4 = job4.CanStart();
			//Assert
			True(result1);
			False(result2);
			False(result3);
			True(result4);
		}
	}
}
