using System;
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

		static ViewModel ()
		{
			_instance = new Lazy<ViewModel>(() => new ViewModel(), LazyThreadSafetyMode.PublicationOnly);
		}

		private ViewModel()
		{
			StartDateTime = DateTime.Now - new TimeSpan(7, 0, 0, 0, 0);
			EndDateTime = DateTime.Now;

			ErrorLogs = new ObservableCollection<ISimpleErrorLog>(){
				new SimpleErrorLog(),
				new SimpleErrorLog(),
				new SimpleErrorLog()
			};
		}

		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }

		public ObservableCollection<ISimpleErrorLog> ErrorLogs { get; set; }

		public IErrorLog ErrorLog { get; set; }
	}
}
