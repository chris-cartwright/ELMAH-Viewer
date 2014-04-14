using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace ELMAH_Viewer.Converters
{
	[TypeConverter]
	public class NotNullConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
