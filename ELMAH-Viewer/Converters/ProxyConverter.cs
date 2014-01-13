using System;
using System.ComponentModel;
using System.Windows.Data;

namespace ELMAH_Viewer.Converters
{
	// Based on http://stackoverflow.com/a/866173/2363358
	[TypeConverter]
	public class ProxyConverter : IValueConverter
	{
		private Type _type;
		private IValueConverter _converter;

		public Type Type
		{
			//get { return _type; }
			set
			{
				if (_type == value)
					return;

				if (value.GetInterface("IValueConverter") == null)
					throw new ArgumentException(string.Format("Type {0} does not implement IValueConverter", value.FullName), "value");

				_type = value;
				_converter = null;
			}
		}

		private void CreateConverter()
		{
			if (_converter != null)
				return;

			if (_type == null)
				throw new InvalidOperationException("Converter type is not defined");

			_converter = Activator.CreateInstance(_type) as IValueConverter;
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			CreateConverter();
			return _converter.Convert(value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			CreateConverter();
			return _converter.ConvertBack(value, targetType, parameter, culture);
		}
	}
}
