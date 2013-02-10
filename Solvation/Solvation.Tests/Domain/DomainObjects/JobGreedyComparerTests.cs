using System;
using NUnit.Framework;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects;

namespace Solvation.Tests.Domain.DomainObjects
{
	[TestFixture]
	public class JobGreedyComparerTests:Assert
	{
		[Test]
		public void ShouldGivePriorityJobWithDependant()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 10, null, 1, 10);
			var job3 = new Job(3, 10, null, 1, 10);
			job1.DependantJobs.Add(job3);
			var comparer = new JobGreedyComparer();
			//Act
			var compareResult = comparer.Compare(job1, job2);
			//Assert
			AreEqual(compareResult, 1);
		}

		[Test]
		public void ShouldGivePriorityJobWithMoreDependant()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 10, null, 1, 10);
			var job3 = new Job(3, 10, null, 1, 10);
			var job4 = new Job(4, 10, null, 1, 10);
			job1.DependantJobs.Add(job2);
			job1.DependantJobs.Add(job3);
			job4.DependantJobs.Add(job3);
			var comparer = new JobGreedyComparer();
			//Act
			var compareResult = comparer.Compare(job1, job4);
			//Assert
			AreEqual(compareResult, 1);
		}

		[Test]
		public void ShouldGivePriorityJobWithBiggerVolume()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 20, null, 1, 10);
			var comparer = new JobGreedyComparer();
			//Act
			var compareResult = comparer.Compare(job1, job2);
			//Assert
			AreEqual(compareResult, -1);
		}

		[Test]
		public void ShouldNotCompareJobsThatCannotStart()
		{
			//Arrange
			var job1 = new Job(1, 10, null, 1, 10);
			var job2 = new Job(2, 10, null, 1, 10);
			var job3 = new Job(3, 10, null, 1, 10);
			var job4 = new Job(4, 10, null, 1, 10);
			job2.PrecedingJobs.Add(job1);
			job3.PrecedingJobs.Add(job2);
			job3.PrecedingJobs.Add(job4);
			var comparer = new JobGreedyComparer();
			//Act and Assert
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
			Throws<InvalidOperationException>(()=>comparer.Compare(job2, job3));
// ReSharper restore ReturnValueOfPureMethodIsNotUsed
		}
	}
}
