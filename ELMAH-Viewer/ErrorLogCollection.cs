using System.Collections.ObjectModel;
using System.ComponentModel;
using ELMAH_Viewer.Annotations;
using ELMAH_Viewer.Common;
using PostSharp.Patterns.Model;

namespace ELMAH_Viewer
{
	[NotifyPropertyChanged]
	public class ErrorLogCollection : ObservableCollection<ISimpleErrorLog>
	{
		public int TotalLogs { get; private set; }
		public int TotalPages { get; private set; }
		public int CurrentPage { get; private set; }

		[UsedImplicitly]
		private void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}
	}
}
