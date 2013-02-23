using System;
using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class DependencyInfoToPointConverter : DependencyObject, IMultiValueConverter
	{
		public static DependencyProperty ScaleDimensionProperty = DependencyProperty.Register("ScaleDimension", typeof(double),
															typeof(DependencyInfoToPointConverter));

		public double ScaleDimension
		{
			get { return (double)GetValue(ScaleDimensionProperty); }
			set { SetValue(ScaleDimensionProperty, value); }
		}

		public double RowHeight { get; set; }
		public double RowSpacing { get; set; }
		public double MinStrokeWidth { get; set; }

		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int rowDifference = (int)values[0];
			var timeDifference = (double)values[1];

			double x = (ScaleDimension * timeDifference>MinStrokeWidth)?ScaleDimension * timeDifference:MinStrokeWidth;
			double y = rowDifference * (RowHeight + RowSpacing) - RowSpacing;

			return new Point(x, y);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
