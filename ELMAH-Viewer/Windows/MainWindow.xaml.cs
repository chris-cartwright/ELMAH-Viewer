using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;
using ELMAH_Viewer.Properties;
using log4net;
using Squirrel;
using MessageBox = System.Windows.MessageBox;

namespace ELMAH_Viewer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
#if DEBUG
        // Used for debugging purposes
        // ReSharper disable once NotAccessedField.Global
        public static MainWindow Self;
#endif

        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainWindow));

        public MainWindow()
        {
            DataContext = ViewModel.Instance;

            InitializeComponent();

            Focus();

#if DEBUG
            Self = this;
#endif
        }

        private SearchParameters CreateSearch()
        {
            SearchParameters sp = new SearchParameters()
            {
                Application = { Mode = SearchItemApplications.SearchMode },
                Host = { Mode = SearchItemHosts.SearchMode },
                Type = { Mode = SearchItemTypes.SearchMode },
                Source = { Mode = SearchItemSources.SearchMode },
                User = { Mode = SearchItemUsers.SearchMode },
                StatusCode = { Mode = SearchItemStatusCodes.SearchMode }
            };

            sp.Application.AddRange(SearchItemApplications.SelectedOptions);
            sp.Host.AddRange(SearchItemHosts.SelectedOptions);
            sp.Type.AddRange(SearchItemTypes.SelectedOptions);
            sp.Source.AddRange(SearchItemSources.SelectedOptions);
            sp.User.AddRange(SearchItemUsers.SelectedOptions);
            sp.StatusCode.AddRange(SearchItemStatusCodes.SelectedOptions.Select(Int32.Parse));

            string searchStr = ViewModel.Instance.SearchString;
            if (!String.IsNullOrWhiteSpace(searchStr))
            {
                Guid guid;
                if (Guid.TryParse(ViewModel.Instance.SearchString, out guid))
                {
                    sp.ErrorId = guid;
                }
                else
                {
                    sp.Contains = searchStr;
                }
            }

            if (ViewModel.Instance.StartDateTime != DateTime.MinValue)
            {
                sp.BeginTimeStamp = ViewModel.Instance.StartDateTime;
            }

            if (ViewModel.Instance.EndDateTime != DateTime.MaxValue)
            {
                sp.EndTimeStamp = ViewModel.Instance.EndDateTime;
            }

            return sp;
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
                _logger.Info(String.Format("Connecting to {0} using connection {1} with settings: {2}", conn.Guid, conn.Name,
                    settings));
                source.Value.Connect(settings);
                ViewModel.Instance.CurrentSource = source;
                ViewModel.Instance.CurrentConnection = conn.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _logger.Error(ex);
            }
        }

        private void CommandBinding_OnSearchExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.Instance.Search(CreateSearch());
        }

        private void CommandBinding_OnResetDatesExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.Instance.StartDateTime = new DateTime(0, DateTimeKind.Utc);
            ViewModel.Instance.EndDateTime = DateTime.MaxValue;
        }

        private void CommandBinding_OnDeleteExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(
                this,
                "Really delete selected logs?",
                "Confirm Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Stop
            );

            if (res != MessageBoxResult.OK)
            {
                return;
            }

            ViewModel.Instance.CurrentSource.Value.DeleteLogs(CreateSearch());
            ViewModel.Instance.LoadSource();
            MessageBox.Show(this, "Logs deleted.", "Success", MessageBoxButton.OK);
            ViewModel.Instance.Search(CreateSearch());
        }

        private void CommandBinding_OnTodayExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.Instance.StartDateTime = DateTime.Now.ToUniversalTime().Date;
            ViewModel.Instance.EndDateTime = ViewModel.Instance.StartDateTime + new TimeSpan(1, 0, 0, 0);
            ViewModel.Instance.RangeLocked = true;
        }

        private void Debug_OnClick(object sender, RoutedEventArgs e)
        {
            Debugger.Break();
        }

        private void ReportBug_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://gitreports.com/issue/chris-cartwright/ELMAH-Viewer");
        }

        private async void CheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            using (UpdateManager mgr = await UpdateManager.GitHubUpdateManager(Settings.Default.UpdateUrl, "ELMAH_Viewer", prerelease: Settings.Default.UsePrerelease))
            {
                if (!mgr.IsInstalledApp)
                {
                    MessageBox.Show(this, "This is not an installed version of ELMAH Viewer. Updates are not available.", "Update Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                ViewModel.Instance.StatusMessage = "Checking for and installing available updates...";
                ReleaseEntry release = await mgr.UpdateApp();
                ViewModel.Instance.StatusMessage = release == null
                    ? "Already latest version."
                    : $"Updated to version {release.Version}. Restart application to use new version.";
            }
        }

        private void RestartApplication_Click(object sender, RoutedEventArgs e)
        {
            UpdateManager.RestartApp();
        }
    }
}
