using System.Collections.Generic;
using System.Globalization;
using DataObjects;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for CreateNewProblem.xaml
	/// </summary>
	public partial class CreateNewProblem
	{
		public CreateNewProblem()
		{
			InitializeComponent();

			var data = GenerateDefaultData();

			ResourceTable.ItemsSource = data.Resources;

			JobCount.txLabel.Content = "Work amount";
			JobCount.txBox.Text = 4.ToString(CultureInfo.InvariantCulture);

			JobTable.ItemsSource = data.Jobs;
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

			return new SchedulingDataContainer(resourceArray, jobArray);
		}
	}
}
