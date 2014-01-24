using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELMAH_Viewer.Common;
using System.ComponentModel.Composition;

namespace ELMAH_Viewer.Sources.SqlServer
{
	[Export(typeof(ILogSource))]
	[ExportMetadata("Name", "SQL Server")]
	public class SqlServer : ILogSource
	{
		public ISet<string> Applications { get; private set; }
		public ISet<string> Hosts { get; private set; }
		public ISet<string> Types { get; private set; }
		public ISet<string> Sources { get; private set; }
		public ISet<string> Users { get; private set; }
		public ISet<int> StatusCodes { get; private set; }

		public void Connect()
		{
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
