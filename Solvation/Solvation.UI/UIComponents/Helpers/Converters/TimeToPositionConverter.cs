using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeToPositionConverter : IValueConverter
	{
		public double ScaleDimention { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//var positionPerSecond = ScaleTimePosition / (ScaleTime - StartTime).TotalSeconds;

			//var time = (DateTime)value;
			//var delta = time - StartTime;
			//return delta.TotalSeconds * positionPerSecond;
			var startTime = (double) value;
			return ScaleDimention * startTime;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
