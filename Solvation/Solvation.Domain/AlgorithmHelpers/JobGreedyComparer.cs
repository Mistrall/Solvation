using System;
using System.Collections.Generic;
using Solvation.Domain.DomainObjects;

namespace Solvation.Domain.AlgorithmHelpers
{
	public class JobGreedyComparer : IComparer<Job>
	{
		private const double Epsilon = 0.00001;
		//Version 1: Only compare dependant jobs and their volumes. Plan bigger first.
		public int Compare(Job x, Job y)
		{
			//We can't compare jobs if one of them cannot start (unfinished preceding jobs)
			if (!x.CanStart()) throw new InvalidOperationException("Cannot compare job x");
			if (!y.CanStart()) throw new InvalidOperationException("Cannot compare job y");

			//Compare valid jobs
			var totalDependantVolume1 = Job.GetFullVolumeWithDependantJobs(x);
			var totalDependantVolume2 = Job.GetFullVolumeWithDependantJobs(y);

			//Float equality should be compared with some arbitrary small number
			if (Math.Abs(totalDependantVolume1 - totalDependantVolume2) < Epsilon)
				if (x.NumberOfDependants > y.NumberOfDependants) return 1;
				
			if ((totalDependantVolume1 - totalDependantVolume2) > Epsilon) return 1;
			
			return -1;
		}
	}
}