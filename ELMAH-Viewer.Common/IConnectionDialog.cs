using System.Windows.Controls;

namespace ELMAH_Viewer.Common
{
	public interface IConnectionDialog
	{
		UserControl Dialog { get; }
		string Name { get; set; }
		string Settings { get; set; }
	}
}
