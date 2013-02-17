using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Solvation.UI.UIComponents.Helpers.Converters
{
	[ContentProperty("Colors")]
	public class RowToColorConverter : IValueConverter
	{
		private List<Color> colors = new List<Color>();

		public List<Color> Colors
		{
			get { return colors; }
			set { colors = value; }
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return colors[((int)value) % colors.Count];
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
