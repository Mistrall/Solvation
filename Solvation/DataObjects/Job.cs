﻿using System.Collections.Generic;

namespace DataObjects
{
	public class Job
	{
		public int Number { get; set; }
		public IEnumerable<int> PrecedingJobs { get; set; }
		public double FullWorkVolume { get; set; }
		public double MinimumIntencity { get; set; }
		public double MaximumIntencity { get; set; }

		public Job(int number, IEnumerable<int> precedingJobs, double fullWorkVolume, double minimumIntencity, double maximumIntencity)
		{
			Number = number;
			PrecedingJobs = precedingJobs;
			FullWorkVolume = fullWorkVolume;
			MinimumIntencity = minimumIntencity;
			MaximumIntencity = maximumIntencity;
		}
	}
}