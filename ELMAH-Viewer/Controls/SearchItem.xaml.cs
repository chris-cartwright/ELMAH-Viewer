using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ELMAH_Viewer.Annotations;
using PostSharp.Patterns.Model;
using System.Linq;

namespace ELMAH_Viewer.Controls
{
	/// <summary>
	/// Interaction logic for SearchItem.xaml
	/// </summary>
	[NotifyPropertyChanged]
	public partial class SearchItem
	{
		public static readonly DependencyProperty SearchOptionsProperty =
			DependencyProperty.Register("SearchOptions", typeof(string[]), typeof(SearchItem));

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
				throw new NotSupportedException();
			}
		}

		public string SearchType { get; set; }

		[IgnoreAutoChangeNotification]
		public string[] SearchOptions
		{
			get { return GetValue(SearchOptionsProperty) as string[]; }
			set {
				SetValue(SearchOptionsProperty, value);
			}
		}

		[SafeForDependencyAnalysis]
		public string[] SelectedOptions
		{
			get
			{
				if (Depends.Guard)
				{
					Depends.On(SearchOptions);
				}

				return CheckboxHolder.FindVisualChildren<CheckBox>().Where(cb => cb.IsChecked == true).Select(cb => cb.Content as string).ToArray();
			}
		}

		public SearchItem()
		{
			InitializeComponent();
			SearchOptions = new string[0];	

			DataContext = this;
		}

		private void All_Click(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox cb in CheckboxHolder.FindVisualChildren<CheckBox>())
			{
				cb.IsChecked = true;
			}
		}

		private void None_Click(object sender, RoutedEventArgs e)
		{
			foreach (CheckBox cb in CheckboxHolder.FindVisualChildren<CheckBox>())
			{
				cb.IsChecked = false;
			}
		}
	}
}
