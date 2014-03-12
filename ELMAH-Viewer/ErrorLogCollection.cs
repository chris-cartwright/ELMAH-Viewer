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
		public long TotalLogs { get; set; }
		public int CurrentPage { get; set; }

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
