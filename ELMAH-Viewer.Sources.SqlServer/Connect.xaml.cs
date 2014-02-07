using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Controls;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer.Sources.SqlServer
{
	/// <summary>
	/// Interaction logic for Connect.xaml
	/// </summary>
	public partial class Connect : IConnectionDialog
	{
		private readonly dynamic _builder;

		public UserControl Dialog {
			get { return this; }
		}

		public string Settings {
			get { return _builder.ConnectionString; }
			set { _builder.ConnectionString = value; }
		}

		string IConnectionDialog.Name {
			get {return NameTextBox.Text; }
			set { NameTextBox.Text = value; }
		}

		public Connect()
		{
			_builder = new NotifyProxy<SqlConnectionStringBuilder>(new SqlConnectionStringBuilder());

			DataContext = _builder;

			InitializeComponent();
		}
	}
}
