using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;
using log4net;
using MessageBox = System.Windows.MessageBox;

namespace ELMAH_Viewer.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MainWindow));

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
			ViewModel.Connection conn = e.Parameter as ViewModel.Connection;
			Debug.Assert(conn != null);

			Lazy<ILogSource, ILogSourceMetadata> source = ViewModel.Instance.LogSources[conn.Guid];

			try
			{
				string settings = SettingsSection.Instance.SavedConnections[conn.Guid, conn.Name];
				Logger.Info(String.Format("Connecting to {0} using connection {1} with settings: {2}", conn.Guid, conn.Name,
					settings));
				source.Value.Connect(settings);
				ViewModel.Instance.CurrentSource = source;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Logger.Error(ex);
			}
		}

		private void Debug_OnClick(object sender, RoutedEventArgs e)
		{
			Debugger.Break();
		}
	}
}
