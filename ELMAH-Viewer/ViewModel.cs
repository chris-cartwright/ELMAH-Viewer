using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using ELMAH_Viewer.Common;
using PostSharp.Patterns.Model;

namespace ELMAH_Viewer
{
	[NotifyPropertyChanged]
	internal class ViewModel
	{
		private static readonly Lazy<ViewModel> _instance;

		[IgnoreAutoChangeNotification]
		public static ViewModel Instance
		{
			get { return _instance.Value; }
		}

		static ViewModel()
		{
			_instance = new Lazy<ViewModel>(() => new ViewModel(), LazyThreadSafetyMode.PublicationOnly);
		}

		private ViewModel()
		{
			StartDateTime = DateTime.Now - new TimeSpan(7, 0, 0, 0, 0);
			EndDateTime = DateTime.Now;

			ErrorLogs = new ObservableCollection<ISimpleErrorLog>();

			Applications = new ObservableCollection<string>();
			Hosts = new ObservableCollection<string>();
			Types = new ObservableCollection<string>();
			Sources = new ObservableCollection<string>();
			Users = new ObservableCollection<string>();
			StatusCodes = new ObservableCollection<int>();

			ErrorLog = new ErrorLog() { TimeUtc = DateTime.Now, StatusCode = 504, Type = typeof(Exception).ToString(), Source = "OMG!" };
		}

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
