using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			DataContext = ViewModel.Instance;

			InitializeComponent();
		}

		private void Connect_OnClick(object sender, RoutedEventArgs e)
		{
			string header = (string) ((MenuItem) e.OriginalSource).Header;
			Lazy<ILogSource, ILogSourceMetadata> source = ViewModel.Instance.LogSources.First(s => s.Metadata.Name == header);
			source.Value.Connect();
		}
	}
}
