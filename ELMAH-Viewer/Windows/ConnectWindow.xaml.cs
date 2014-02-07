using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace ELMAH_Viewer.Windows
{
	/// <summary>
	/// Interaction logic for ConnectWindow.xaml
	/// </summary>
	public partial class ConnectWindow
	{
		public bool Created { get; private set; }

		private void CommandBinding_OnCancelExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Created = false;
			Close();
		}

		private void CommandBinding_OnConnectExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			Created = true;
			Close();
		}

		public ConnectWindow()
		{
			InitializeComponent();
		}
	}
}
