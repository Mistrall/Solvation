using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Solvation.UI.Models
{
	public class JobModel
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
				if (!String.IsNullOrEmpty(value))
				{
					PrecedingJobs = value.Split(',').Select(Int32.Parse).ToList();	
				}
			} 
		}
		public double FullWorkVolume { get; set; }
		public double MinimumIntensity { get; set; }
		public double MaximumIntensity { get; set; }

		public JobModel(int number):this(number, string.Empty, 0, 0, 0)
		{}

		public JobModel(int number, string precedingJobs, double fullWorkVolume, double minimumIntencity, double maximumIntensity)
		{
			Number = number;
			PrecedingJobs=new List<int>();
			PrecedingJobsStr = precedingJobs;
			FullWorkVolume = fullWorkVolume;
			MinimumIntensity = minimumIntencity;
			MaximumIntensity = maximumIntensity;
		}
	}
}