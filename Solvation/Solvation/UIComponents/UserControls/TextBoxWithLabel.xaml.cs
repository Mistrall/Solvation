using System.Windows;

namespace Solvation.UI.UIComponents.UserControls
{
	/// <summary>
	/// Interaction logic for TextBoxWithLabel.xaml
	/// </summary>
	public partial class TextBoxWithLabel
	{
		public TextBoxWithLabel()
		{
			InitializeComponent();
			LayoutRoot.DataContext = this;
		}

		#region Label DP
		public static DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof(string), typeof(TextBoxWithLabel));

		public string Label
		{
			get { return (string) GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value);}
		}
		#endregion

		#region Value DP

		public static DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(TextBoxWithLabel), new PropertyMetadata(null));

		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		#endregion

		public event RoutedEventHandler CustomClick;

		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			if (CustomClick!=null) CustomClick(this, new RoutedEventArgs());
		}
	}
}
