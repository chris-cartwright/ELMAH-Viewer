using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ELMAH_Viewer.Annotations;
using ELMAH_Viewer.Common;
using ELMAH_Viewer.Configuration;
using PostSharp.Patterns.Model;

namespace ELMAH_Viewer
{
	[NotifyPropertyChanged]
	public class ErrorLogCollection : ObservableCollection<ISimpleErrorLog>
	{
		private int _currentPage;

		public long TotalLogs { get; set; }

		public int CurrentPage
		{
			get { return _currentPage; }
			set
			{
				if (value > TotalPages || value <= 0)
				{
					return;
				}

				_currentPage = value;
			}
		}

		public int TotalPages
		{
			get
			{
				Depends.On(TotalLogs);
				return (int)Math.Ceiling((double)TotalLogs / SettingsSection.Instance.Results.ResultsPerPage);
			}
		}

		[UsedImplicitly]
		private void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
	}
}
