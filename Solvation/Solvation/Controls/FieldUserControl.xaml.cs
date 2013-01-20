using System.Windows;

namespace Solvation.Controls
{
	/// <summary>
	/// Interaction logic for FieldUserControl.xaml
	/// </summary>
	public partial class FieldUserControl
	{
		public FieldUserControl()
		{
			InitializeComponent();

			LayoutRoot.DataContext = this;
		}

		#region Label DP
		public static DependencyProperty LabelProperty =
			DependencyProperty.Register("Label", typeof (string), typeof (FieldUserControl));


		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value);}
		}

		#endregion
		#region Value DP

		///<summary>
		///Gets or sets the Value which is being displayed
		///</summary>
		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		/// <summary>
		/// Identified the Label dependency property
		/// </summary>
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object),
			  typeof(FieldUserControl), new PropertyMetadata(null));

		#endregion
	}
}
