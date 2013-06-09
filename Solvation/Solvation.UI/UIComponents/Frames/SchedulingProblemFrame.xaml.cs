using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.DomainObjects.Simplex;
using Solvation.Domain.Services;
using Solvation.UI.Models;

namespace Solvation.UI.UIComponents.Frames
{
	/// <summary>
	/// Interaction logic for SchedulingProblemFrame.xaml
	/// </summary>
	public partial class SchedulingProblemFrame
	{
		private PlanModel model;
		private readonly NewProblemModel initialData;

		public SchedulingProblemFrame():this(null, null)
		{}

		public SchedulingProblemFrame(PlanModel planModel, NewProblemModel initialData)
		{
			InitializeComponent();
			LayoutRoot.DataContext = model = planModel ?? CreateFakePlanModel();
			this.initialData = initialData;
		}

		private PlanModel CreateFakePlanModel()
		{
			var firstJob = new Job(1, 10, null, .1, 2);
			var secondJob = new Job(2, 10, new List<int> { 1 }, .1, 2);
			var thirdJob = new Job(3, 20, new List<int> { 1, 2 }, .1, 2);
			var fourthJob = new Job(4, 20, null, .1, 2);
			var jobs = new List<Job> { firstJob, secondJob, thirdJob, fourthJob };
			var resource1 = new Resource(1, 20);
			var resources = new List<Resource> { resource1 };
			var dependencies = new List<double[]> { new double[] { 7 }, new double[] { 14 }, new double[] { 10 }, new double[] { 6 } };
			var planBuilder = new JobPlanBuilder();
			
			var plan = planBuilder.GetBasePlan(resources, jobs, dependencies);

			var planModel = new PlanModel(plan);

			return planModel;
		}

		public void OptimizePlan()
		{
			var resources = (initialData.Resources.Select(rm => new Resource(rm.Number, rm.Value))).ToList();
			var jobs =
				(initialData.Jobs.Select(
					jm => new Job(jm.Number, jm.FullWorkVolume, jm.PrecedingJobs, jm.MinimumIntensity, jm.MaximumIntensity))).ToList();
			var simplexTuple = (new SimplexInputBuilder()).BuildFromBasePlan(model.Plan.ToList(), jobs, resources, initialData.DependencyValues.ToList());

			SimplexResult simplexResult = (new SimplexSolver()).Solve(simplexTuple);
		}

		private void OptimizePlanClick(object sender, RoutedEventArgs e)
		{
			OptimizePlan();
		}
	}
}
