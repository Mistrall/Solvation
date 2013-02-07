using System;
using System.Collections.Generic;
using System.Windows;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.Services;
using Solvation.Models;
using Solvation.UI.Models;
using Solvation.WPF.MDI;

namespace Solvation.UI.UIComponents.Frames
{
	/// <summary>
	/// Interaction logic for CreateNewProblem.xaml
	/// </summary>
	public partial class CreateNewProblem
	{
		private readonly MdiContainer parent;
		private NewProblemModel model;

		public JobPlanBuilder PlanBuilder { get; set; } 

		public CreateNewProblem()
		{
			//TODO: replace with DI property injection
			PlanBuilder=new JobPlanBuilder();

			InitializeComponent();

			model = GenerateDefaultDataModel(); 
			DataContext = model;
		}

		public CreateNewProblem(MdiContainer parent):this()
		{
			this.parent = parent;
		}

		private NewProblemModel GenerateDefaultDataModel()
		{
			var resourceArray = new List<Resource> {new Resource(1, 20), new Resource(2, 50)};

			var jobArray = new List<Job>
				{
					new Job(1, null, 100, 1, 10),
					new Job(2, "1", 50, 1, 10),
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

			return new NewProblemModel(2, 4, resourceArray, jobArray, dependencies);
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

			model = new NewProblemModel(newResourceCount, model.JobCount, newResourceList, model.Jobs, model.DependencyValues);

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

			model = new NewProblemModel(model.ResourceCount, newJobsCount, model.Resources, newJobsList, model.DependencyValues);

			DataContext = model;
		}

		//Hack, we need to raise some event for parent here
		private void StartThisProblemClick(object sender, RoutedEventArgs e)
		{
			var baseStepList = PlanBuilder.GetBasePlan(model.Resources, model.Jobs, model.DependencyValues);

			var problemFrame = new MdiChild
			{
				Title = "Scheduling problem",
				Content = new SchedulingProblemFrame(new PlanModel(baseStepList)),
				Width = 800,
				Height = 600,
				Position = new Point(300, 300)
			};

			parent.Children.Add(problemFrame);
		}
	}
}
