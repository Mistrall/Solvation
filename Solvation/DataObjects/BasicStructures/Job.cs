using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Solvation.Domain.BasicStructures
{
	public class Job
	{
		private IEnumerable<int> precedingJobs; 

		public int Number { get; set; }
		public IEnumerable<int> PrecedingJobs 
		{ 
			get { return precedingJobs; }
			set { precedingJobs = value; }
		}
		public string PrecedingJobsStr 
		{ 
			get
			{
				return String.Join(",", precedingJobs.Select(j => j.ToString(CultureInfo.InvariantCulture)));
			}
			set
			{
				if (value != null)
				{
					PrecedingJobs = value.Split(',').Select(Int32.Parse).ToList();	
				}
			} 
		}
		public double FullWorkVolume { get; set; }
		public double MinimumIntencity { get; set; }
		public double MaximumIntencity { get; set; }

		public Job(int number):this(number, string.Empty, 0, 0, 0)
		{}

		public Job(int number, string precedingJobs, double fullWorkVolume, double minimumIntencity, double maximumIntencity)
		{
			Number = number;
			PrecedingJobs=new List<int>();
			PrecedingJobsStr = precedingJobs;
			FullWorkVolume = fullWorkVolume;
			MinimumIntencity = minimumIntencity;
			MaximumIntencity = maximumIntencity;
		}
	}
}