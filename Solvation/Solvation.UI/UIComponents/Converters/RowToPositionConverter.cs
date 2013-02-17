using System;
using System.Globalization;
using System.Windows.Data;

namespace Solvation.UI.UIComponents.Converters
{
	public class RowToPositionConverter : IValueConverter
	{
		public double RowHeight { get; set; }
		public double RowSpacing { get; set; }
		public double Offset { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((int)value * (RowHeight + RowSpacing)) - RowSpacing + Offset;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
