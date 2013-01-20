using Solvation.Models;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for TestFrame.xaml
	/// </summary>
	public partial class TestFrame
	{
		public TestFrame()
		{
			InitializeComponent();
			var model = new ShoesizeModel()
			{
				Shoesize = 12,
				Height = 34.5
			};

			DataContext = model;
		}
	}
}
