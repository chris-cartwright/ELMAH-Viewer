using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;

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

		private void CommandBinding_OnCreateConnectionExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter == null)
			{
				return;
			}

			Lazy<ILogSource, ILogSourceMetadata> source = ViewModel.Instance.LogSources.First(s => s.Metadata.Guid == e.Parameter.ToString());
			IConnectionDialog dlg = source.Value.GetConnectionDialog();

			ConnectWindow connect = new ConnectWindow()
			{
				Owner = this,
				Title = source.Metadata.Name
			};

			connect.Grid.Children.Add(dlg.Dialog);

			connect.Focus();
			connect.ShowDialog();

			if (!connect.Created)
			{
				return;
			}

			// Save/update connection
			ConnectionElement connection = SettingsSection.Instance.SavedConnections.GetItemByKey(dlg.Name);

			if (connection == null)
			{
				connection = new ConnectionElement() { Name = dlg.Name };
				SettingsSection.Instance.SavedConnections.Add(connection);
			}

			connection.Provider = source.Metadata.Guid;
			connection.Content = dlg.Settings;
			SettingsSection.Save();
			MessageBox.Show("Created connection");
		}

		private void CommandBinding_OnConnectExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			bool temp = false;
		}
	}
}
