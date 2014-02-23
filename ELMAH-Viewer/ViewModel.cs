using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;
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

		public static RoutedUICommand CreateConnection { get; private set; }
		public static RoutedUICommand Connect { get; private set; }

		[IgnoreAutoChangeNotification]
		public static ViewModel Instance
		{
			get { return _instance.Value; }
		}

		static ViewModel()
		{
			CreateConnection = new RoutedUICommand("Create new connection", "CreateConnectionCommand", typeof(ViewModel));
			Connect = new RoutedUICommand("Connect to source", "ConnectCommand", typeof(ViewModel));

			_instance = new Lazy<ViewModel>(() => new ViewModel(), LazyThreadSafetyMode.PublicationOnly);
		}

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

		public Dictionary<string, List<Connection>> SavedConnections { get; set; }

		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }

		public ObservableCollection<string> Applications { get; set; }
		public ObservableCollection<string> Hosts { get; set; }
		public ObservableCollection<string> Types { get; set; }
		public ObservableCollection<string> Sources { get; set; }
		public ObservableCollection<string> Users { get; set; }
		public ObservableCollection<int> StatusCodes { get; set; }

		public ObservableCollection<ISimpleErrorLog> ErrorLogs { get; set; }

		public ErrorLog ErrorLog { get; set; }

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

			StartDateTime = DateTime.Now - new TimeSpan(7, 0, 0, 0, 0);
			EndDateTime = DateTime.Now;

			ErrorLogs = new ObservableCollection<ISimpleErrorLog>();

			Applications = new ObservableCollection<string>();
			Hosts = new ObservableCollection<string>();
			Types = new ObservableCollection<string>();
			Sources = new ObservableCollection<string>();
			Users = new ObservableCollection<string>();
			StatusCodes = new ObservableCollection<int>();

			ErrorLog = new ErrorLog();
		}

		public void LoadSource()
		{
			
		}
	}
}
