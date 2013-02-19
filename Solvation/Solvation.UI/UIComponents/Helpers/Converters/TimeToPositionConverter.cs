using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeToPositionConverter : DependencyObject, IValueConverter
	{
		public static DependencyProperty ScaleDimensionProperty =
			 DependencyProperty.Register("ScaleDimension", typeof(double),
			 typeof(TimeToPositionConverter));

		public double ScaleDimension
		{
			get { return (double)GetValue(ScaleDimensionProperty); }
			set { SetValue(ScaleDimensionProperty, value); }
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var startTime = (double) value;
			return ScaleDimension * startTime;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
