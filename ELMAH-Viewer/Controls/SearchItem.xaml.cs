using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using ELMAH_Viewer.Annotations;

namespace ELMAH_Viewer.Controls
{
	/// <summary>
	/// Interaction logic for SearchItem.xaml
	/// </summary>
	public partial class SearchItem : UserControl
	{
		[UsedImplicitly]
		[TypeConverter]
		public class ListBoxDisabledConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				return ((ComboBoxItem)value).Content.ToString() != "Disabled";
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}

		public SearchItem()
		{
			InitializeComponent();
		}
	}
}
