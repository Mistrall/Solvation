using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeSpanToDimensionConverter : IValueConverter
	{
		public double ScaleDimention { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var duration = (double)value;
			return ScaleDimention * duration;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
