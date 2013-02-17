using System;
using System.Globalization;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeSpanToDimensionConverter : IValueConverter
	{
		public TimeSpan ScaleTimeSpan { get; set; }
		public double ScaleDimension { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var timeSpan = (TimeSpan)value;
			return ScaleDimension * timeSpan.TotalSeconds / ScaleTimeSpan.TotalSeconds;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
