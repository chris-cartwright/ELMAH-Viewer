using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Properties;

namespace ELMAH_Viewer.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			DataContext = ViewModel.Instance;

			InitializeComponent();

			Focus();
		}

		private void Connect_OnClick(object sender, RoutedEventArgs e)
		{
			string header = (string)((MenuItem)e.OriginalSource).Header;
			Lazy<ILogSource, ILogSourceMetadata> source = ViewModel.Instance.LogSources.First(s => s.Metadata.Name == header);
			IConnectionDialog dlg = source.Value.GetConnectionDialog();

			ConnectWindow connect = new ConnectWindow()
			{
				Owner = this,
				Title = source.Metadata.Name
			};

			connect.Grid.Children.Add(dlg.Dialog);

			connect.Focus();
			connect.ShowDialog();

			if (connect.Created)
			{
				MessageBox.Show("Created connection");
				// Save/update connection
			}
		}
	}
}
