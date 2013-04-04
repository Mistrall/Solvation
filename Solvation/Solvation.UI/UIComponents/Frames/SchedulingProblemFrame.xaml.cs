using System.Collections.Generic;
using System.Linq;
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

		public SchedulingProblemFrame():this(null)
		{}

		public SchedulingProblemFrame(PlanModel planModel)
		{
			InitializeComponent();
			LayoutRoot.DataContext = model = planModel ?? CreateFakePlanModel();
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
			var simplexTuple = (new SimplexInputBuilder()).BuildFromBasePlan(model.Plan.ToList());
			var optimizedPlan = (new SimplexSolver()).Solve(simplexTuple);
		}
	}
}
