using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeSpanToDimensionConverter : DependencyObject, IValueConverter
	{
		public static DependencyProperty ScaleDimensionProperty = DependencyProperty.Register("ScaleDimension", typeof(double),
																	typeof(TimeSpanToDimensionConverter));

		public double ScaleDimension
		{
			get { return (double)GetValue(ScaleDimensionProperty); }
			set { SetValue(ScaleDimensionProperty, value); }
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var duration = (double)value;
			return ScaleDimension * duration;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
