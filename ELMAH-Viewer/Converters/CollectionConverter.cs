using System;
using System.Collections;
using System.Collections.Generic;
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

			if (!typeof(ICollection).IsAssignableFrom(targetType))
			{
				throw new ArgumentException("Target type must inherit from ICollection", "targetType");
			}

			Type basic = targetType.IsGenericType ? targetType.GenericTypeArguments[0] : targetType.GetElementType();
			IList ret = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(basic));
			foreach (object item in value as IEnumerable)
			{
				ret.Add(System.Convert.ChangeType(item, basic));
			}

			if (targetType.IsGenericType)
			{
				return ret;
			}

			Array array = Array.CreateInstance(targetType.GetElementType(), ret.Count);
			ret.CopyTo(array, 0);
			return array;
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
