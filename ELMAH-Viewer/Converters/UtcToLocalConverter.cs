using System;
using System.Globalization;
using System.Windows.Data;

namespace ELMAH_Viewer.Converters
{
	public class UtcToLocalConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(DateTime))
			{
				throw new NotSupportedException();
			}

			DateTime dt = (DateTime)value;
			return dt.ToLocalTime();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
