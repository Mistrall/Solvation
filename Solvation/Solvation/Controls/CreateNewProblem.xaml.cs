using System;
using System.Collections.Generic;
using System.Globalization;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for CreateNewProblem.xaml
	/// </summary>
	public partial class CreateNewProblem
	{
		protected List<Resource> ResourceArray;
		protected List<Job> JobArray;

		public CreateNewProblem()
		{
			InitializeComponent();
			
			ResourceArray = new List<Resource>();
			ResourceArray.Add(new Resource(1, 20));
			ResourceArray.Add(new Resource(2, 50));

			ResourceTable.ItemsSource = ResourceArray;

			JobCount.txLabel.Content = "Work amount";
			JobCount.txBox.Text = 4.ToString(CultureInfo.InvariantCulture);

			JobArray = new List<Job>();
			JobArray.Add(new Job(1, null, 100, 1, 10));
			JobArray.Add(new Job(2, new[] {1}, 50, 1, 10));
			JobArray.Add(new Job(3, null, 50, 1, 10));
			JobArray.Add(new Job(4, null, 80, 1, 10));

			JobTable.ItemsSource = JobArray;
		}

		protected class Resource
		{
			public int Number { get; set; }
			public double Value { get; set; }
			public Resource(int n, double v)
			{
				Number = n;
				Value = v;
			}
		}

		protected class Job
		{
			public int Number { get; set; }
			public IEnumerable<Int32> PrecedingJobs { get; set; }
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
}
