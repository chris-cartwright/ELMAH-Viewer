using System;
using System.Globalization;
using System.Windows.Data;

namespace ELMAH_Viewer.Converters
{
	public class LocalToUtcConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			if (value.GetType() != typeof(DateTime))
			{
				throw new NotSupportedException();
			}

			DateTime dt = (DateTime)value;
			return dt.ToUniversalTime();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			if (value.GetType() != typeof(DateTime))
			{
				throw new NotSupportedException();
			}

			DateTime dt = (DateTime)value;
			return dt.ToLocalTime();
		}
	}
}
