using System.Windows;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for TextBoxWithLabel.xaml
	/// </summary>
	public partial class TextBoxWithLabel
	{
		public TextBoxWithLabel()
		{
			InitializeComponent();
		}

		public string LabelText
		{
			get { return txLabel.Content.ToString(); }
			set { txLabel.Content = value; }
		}
	}
}
