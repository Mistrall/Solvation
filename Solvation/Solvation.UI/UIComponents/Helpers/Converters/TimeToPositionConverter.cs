using System;
using System.Globalization;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	public class TimeToPositionConverter : IValueConverter
	{
		//public DateTime StartTime { get; set; }
		//public DateTime ScaleTime { get; set; }
		public double Duration { get; set; }
		public double ScaleTimePosition { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			//var positionPerSecond = ScaleTimePosition / (ScaleTime - StartTime).TotalSeconds;

			//var time = (DateTime)value;
			//var delta = time - StartTime;
			//return delta.TotalSeconds * positionPerSecond;

			return ScaleTimePosition/Duration;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
