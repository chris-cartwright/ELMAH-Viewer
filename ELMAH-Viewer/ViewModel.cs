using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using ELMAH_Viewer.Common;
using PostSharp.Patterns.Model;

namespace ELMAH_Viewer
{
	[NotifyPropertyChanged]
	internal class ViewModel
	{
		private static readonly Lazy<ViewModel> _instance;

		public static RoutedUICommand Connect { get; private set; }

		[IgnoreAutoChangeNotification]
		public static ViewModel Instance
		{
			get { return _instance.Value; }
		}

		static ViewModel()
		{
			Connect = new RoutedUICommand("Connect to source", "ConnectCommand", typeof(Commands));

			_instance = new Lazy<ViewModel>(() => new ViewModel(), LazyThreadSafetyMode.PublicationOnly);
		}

		private ViewModel()
		{
			string path = Path.Combine(Helpers.ApplicationPath, Configuration.SettingsSection.Instance.Sources.Location);
			AggregateCatalog catalog = new AggregateCatalog();
			catalog.Catalogs.Add(new DirectoryCatalog(path));

			CompositionContainer container = new CompositionContainer(catalog);

			try
			{
				container.ComposeParts(this);
				if (LogSources.Length == 0)
				{
					throw new ApplicationException("Could not find any log sources.");
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Could not load error sources. Exiting application.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Application.Current.Shutdown();
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

		[ImportMany(typeof(ILogSource))]
		public Lazy<ILogSource, ILogSourceMetadata>[] LogSources { get; set; }

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
	}
}
