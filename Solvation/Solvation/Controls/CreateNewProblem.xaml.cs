using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DataObjects;
using DataObjects.BasicStructures;
using Solvation.Models;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for CreateNewProblem.xaml
	/// </summary>
	public partial class CreateNewProblem
	{
		private NewProblemModel model;

		public CreateNewProblem()
		{
			InitializeComponent();

			var data = GenerateDefaultData();

			model = new NewProblemModel(2, 4, data.Resources, data.Jobs);

			DataContext = model;
		}

		private SchedulingDataContainer GenerateDefaultData()
		{
			var resourceArray = new List<Resource> {new Resource(1, 20), new Resource(2, 50)};

			var jobArray = new List<Job>
				{
					new Job(1, null, 100, 1, 10),
					new Job(2, new[] {1}, 50, 1, 10),
					new Job(3, null, 50, 1, 10),
					new Job(4, null, 80, 1, 10)
				};

			var dependencies = new List<double[]>
				{
					new double[] {1, 2},
					new double[] {2, 2},
					new double[] {0.7, 3.5},
					new double[] {2, 4}
				};


			return new SchedulingDataContainer(resourceArray, jobArray, dependencies);
		}

		private void OnResourceCountClick(object sender, RoutedEventArgs e)
		{
			var newResourceCount = Int32.Parse(ResourceCount.Value.ToString());
			if (newResourceCount == model.ResourceCount) return;

			var newResourceList = new List<Resource>();


			if (newResourceCount > model.ResourceCount)
			{
				newResourceList.AddRange(model.Resources);

				for (int i = model.ResourceCount; i < newResourceCount; i++) newResourceList.Add(new Resource(i + 1));
			}
			else
			{
				for (int i = 0; i < newResourceCount; i++)
					newResourceList.Add(model.Resources[i]);
			}

			model = new NewProblemModel(newResourceCount, model.JobCount, newResourceList, model.Jobs);

			DataContext = model;
		}


		private void OnJobCountClick(object sender, RoutedEventArgs e)
		{
			var newJobsCount = Int32.Parse(JobCount.Value.ToString());
			if (newJobsCount == model.JobCount) return;

			var newJobsList = new List<Job>();

			if (newJobsCount > model.JobCount)
			{
				newJobsList.AddRange(model.Jobs);

				for (int i = model.JobCount; i < newJobsCount; i++) newJobsList.Add(new Job(i + 1));
			}
			else
			{
				for (int i = 0; i < newJobsCount; i++)
					newJobsList.Add(model.Jobs[i]);
			}

			model = new NewProblemModel(model.ResourceCount, newJobsCount, model.Resources, newJobsList);

			DataContext = model;
		}
	}
}
