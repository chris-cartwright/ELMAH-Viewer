using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace ELMAH_Viewer.Converters
{
	[TypeConverter]
	public class CollectionConverter : IValueConverter
	{
		private object ConvertCollection(object value, Type targetType)
		{
			if (!(value is IEnumerable))
			{
				throw new ArgumentException("Value must implement IEnumerable", "value");
			}

			if (!targetType.IsGenericType)
			{
				throw new ArgumentException("Target type must have at least one generic type", "targetType");
			}

			if (!typeof(IList).IsAssignableFrom(targetType))
			{
				throw new ArgumentException("Target type must inherit from IList", "targetType");
			}

			IList ret = (IList)Activator.CreateInstance(targetType);
			foreach (object item in value as IEnumerable)
			{
				ret.Add(System.Convert.ChangeType(item, targetType.GenericTypeArguments[0]));
			}

			return ret;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertCollection(value, targetType);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ConvertCollection(value, targetType);
		}
	}
}
