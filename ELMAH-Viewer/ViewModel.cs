using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;
using PostSharp;
using PostSharp.Patterns.Model;

namespace ELMAH_Viewer
{
	[NotifyPropertyChanged]
	internal class ViewModel
	{
		[NotifyPropertyChanged]
		public class Connection
		{
			public string Name { get; set; }
			public string Guid { get; set; }
		}

		private static readonly Lazy<ViewModel> _instance;
		private Lazy<ILogSource, ILogSourceMetadata> _currentSource;

		public static RoutedUICommand CreateConnectionCommand { get; private set; }
		public static RoutedUICommand ConnectCommand { get; private set; }
		public static RoutedUICommand SearchCommand { get; private set; }
		public static RoutedUICommand ResetDatesCommand { get; private set; }
		public static RoutedUICommand DeleteCommand { get; private set; }

		[IgnoreAutoChangeNotification]
		public static ViewModel Instance
		{
			get { return _instance.Value; }
		}

		public static Visibility IsDebug
		{
#if DEBUG
			get { return Visibility.Visible; }
#else
			get { return Visibility.Collapsed; }
#endif
		}

		static ViewModel()
		{
			CreateConnectionCommand = new RoutedUICommand("Create new connection", "CreateConnectionCommand", typeof(ViewModel));
			ConnectCommand = new RoutedUICommand("Connect to source", "ConnectCommand", typeof(ViewModel));
			SearchCommand = new RoutedUICommand("Search logs", "SearchCommand", typeof(ViewModel));
			ResetDatesCommand = new RoutedUICommand("Reset Dates", "ResetDatesCommand", typeof(ViewModel));
			DeleteCommand = new RoutedUICommand("Delete logs", "DeleteCommand", typeof(ViewModel));

			_instance = new Lazy<ViewModel>(() => new ViewModel(), LazyThreadSafetyMode.PublicationOnly);
		}

		private IResult _logs;

		[ImportMany(typeof(ILogSource))]
		public LogSourceCollection LogSources { get; set; }

		public Lazy<ILogSource, ILogSourceMetadata> CurrentSource
		{
			get { return _currentSource; }
			set
			{
				_currentSource = value;
				LoadSource();
			}
		}

		public ISimpleErrorLog SelectedLog
		{
			set
			{
				if (value == null)
				{
					return;
				}

				LoadLog(value.ErrorId);
			}
		}

		public string WindowTitle
		{
			get
			{
				Depends.On(CurrentConnection);
				return CurrentSource == null ? "ELMAH-Viewer" : "ELMAH-Viewer :: " + CurrentConnection;
			}
		}

		public Dictionary<string, List<Connection>> SavedConnections { get; set; }
		public string CurrentConnection { get; set; }

		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }

		public string[] Applications { get; set; }
		public string[] Hosts { get; set; }
		public string[] Types { get; set; }
		public string[] Sources { get; set; }
		public string[] Users { get; set; }
		public int[] StatusCodes { get; set; }

		public ErrorLogCollection ErrorLogs { get; set; }
		public ErrorLog ErrorLog { get; set; }

		private async void LoadLog(Guid errorId)
		{
			IErrorLog log = await _currentSource.Value.GetLog(errorId);
			ErrorLog = new ErrorLog(log);
		}

		private void ErrorLogsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName != "CurrentPage")
			{
				return;
			}

			LoadPage(ErrorLogs.CurrentPage);
		}

		private async void LoadPage(int page)
		{
			if (_logs == null)
			{
				return;
			}

			ErrorLogs.Clear();

			IResultPage p = await _logs.GetPageAsync(page);
			if (p.HasItems)
			{
				ErrorLogs.CurrentPage = p.Page;
				ErrorLogs.TotalLogs = _logs.TotalResults;
				ErrorLogs.AddRange(p.Items);
			}
		}

		private ViewModel()
		{
			string path = Path.Combine(Helpers.ApplicationPath, SettingsSection.Instance.Sources.Location);
			AggregateCatalog catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new DirectoryCatalog(path));

			CompositionContainer container = new CompositionContainer(catalog);

			try
			{
				container.ComposeParts(this);
				if (LogSources.Count == 0)
				{
					throw new ApplicationException("Could not find any log sources.");
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Could not load error sources. Exiting application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Application.Current.Shutdown();
			}

			SavedConnections = new Dictionary<string, List<Connection>>();
			foreach (ConnectionElement conn in SettingsSection.Instance.SavedConnections)
			{
				Lazy<ILogSource, ILogSourceMetadata> provider = LogSources[conn.Provider];
				string providerName = provider.Metadata.Name;

				if (!SavedConnections.ContainsKey(providerName))
				{
					SavedConnections[providerName] = new List<Connection>();
				}

				SavedConnections[providerName].Add(new Connection() { Guid = conn.Provider, Name = conn.Name });
			}

			StartDateTime = DateTime.MinValue;
			EndDateTime = DateTime.MaxValue;

			ErrorLogs = new ErrorLogCollection();
			INotifyPropertyChanged el = Post.Cast<ErrorLogCollection, INotifyPropertyChanged>(ErrorLogs);
			el.PropertyChanged += ErrorLogsPropertyChanged;

			Applications = new string[0];
			Hosts = new string[0];
			Types = new string[0];
			Sources = new string[0];
			Users = new string[0];
			StatusCodes = new int[0];

			ErrorLog = new ErrorLog();
		}

		public void LoadSource()
		{
			_currentSource.Value.LoadSearchValues();

			Applications = _currentSource.Value.Applications.ToArray();
			Hosts = _currentSource.Value.Hosts.ToArray();
			Types = _currentSource.Value.Types.ToArray();
			Sources = _currentSource.Value.Sources.ToArray();
			Users = _currentSource.Value.Users.ToArray();
			StatusCodes = _currentSource.Value.StatusCodes.ToArray();

			_logs = _currentSource.Value.GetLogs(SettingsSection.Instance.Results.ResultsPerPage);

			ErrorLogs.CurrentPage = 1;
		}

		public void Search(SearchParameters sp)
		{
			_logs = _currentSource.Value.GetLogs(SettingsSection.Instance.Results.ResultsPerPage, sp);
			LoadPage(1);
		}
	}
}
