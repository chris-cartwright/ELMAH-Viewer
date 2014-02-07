using System.Windows.Input;

namespace ELMAH_Viewer.Common
{
	public static class Commands
	{
		public static readonly RoutedUICommand Connect;
		public static readonly RoutedUICommand Cancel;

		static Commands()
		{
			Connect = new RoutedUICommand("Connect to source", "ConnectCommand", typeof(Commands));
			Cancel = new RoutedUICommand("Cancel creating connection", "CancelCommand", typeof(Commands));
		}
	}
}
