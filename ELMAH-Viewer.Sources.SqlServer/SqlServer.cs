using System;
using System.Collections.Generic;
using ELMAH_Viewer.Common;
using log4net;

namespace ELMAH_Viewer.Sources.SqlServer
{
	[ExportPlugin("SQL Server", "97d260c7-e5ad-4885-89ba-88abcc8673d4")]
	public class SqlServer : ILogSource
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(SqlServer));

		public ISet<string> Applications { get; private set; }
		public ISet<string> Hosts { get; private set; }
		public ISet<string> Types { get; private set; }
		public ISet<string> Sources { get; private set; }
		public ISet<string> Users { get; private set; }
		public ISet<int> StatusCodes { get; private set; }

		public IConnectionDialog GetConnectionDialog()
		{
			return new Connect();
		}

		public void Connect(string settings)
		{
			Logger.Info(String.Format("Connecting with settings: {0}", settings));
			throw new NotImplementedException();
		}

		public ISimpleErrorLog[] GetLogs(int count, int offset)
		{
			throw new NotImplementedException();
		}

		public ISimpleErrorLog[] SearchLogs(int count, int offset, ISearchParameters parameters)
		{
			throw new NotImplementedException();
		}
	}
}
