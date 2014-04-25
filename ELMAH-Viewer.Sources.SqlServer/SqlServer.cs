using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	[ExportPlugin("SQL Server", "97d260c7-e5ad-4885-89ba-88abcc8673d4")]
	public class SqlServer : ILogSource
	{
		private Database _connection;

		public ISet<string> Applications { get; private set; }
		public ISet<string> Hosts { get; private set; }
		public ISet<string> Types { get; private set; }
		public ISet<string> Sources { get; private set; }
		public ISet<string> Users { get; private set; }
		public ISet<int> StatusCodes { get; private set; }

		public SqlServer()
		{
			Applications = new HashSet<string>();
			Hosts = new HashSet<string>();
			Types = new HashSet<string>();
			Sources = new HashSet<string>();
			Users = new HashSet<string>();
			StatusCodes = new HashSet<int>();
		}

		public IConnectionDialog GetConnectionDialog()
		{
			return new Connect();
		}

		public void LoadSearchValues()
		{
            Applications.Clear();
		    Hosts.Clear();
		    Types.Clear();
		    Sources.Clear();
		    Users.Clear();
		    StatusCodes.Clear();

			Applications.AddRange(_connection.Fetch<string>("SELECT DISTINCT Application FROM ELMAH_Error ORDER BY Application"));
			Hosts.AddRange(_connection.Fetch<string>("SELECT DISTINCT Host FROM ELMAH_Error ORDER BY Host"));
			Types.AddRange(_connection.Fetch<string>("SELECT DISTINCT Type FROM ELMAH_Error ORDER BY Type"));
			Sources.AddRange(_connection.Fetch<string>("SELECT DISTINCT Source FROM ELMAH_Error ORDER BY Source"));
			Users.AddRange(_connection.Fetch<string>("SELECT DISTINCT User FROM ELMAH_Error ORDER BY User"));
			StatusCodes.AddRange(_connection.Fetch<int>("SELECT DISTINCT StatusCode FROM ELMAH_Error ORDER BY StatusCode"));
		}

		public async Task<IErrorLog> GetLog(Guid errorId)
		{
			return await Task.Factory.StartNew<IErrorLog>(() => _connection.SingleOrDefault<Elmah>("SELECT * FROM ELMAH_Error WHERE ErrorId=@0", errorId));
		}

		public IResult GetLogs(int resultsPage)
		{
			return new ResultLoader(_connection, resultsPage);
		}

		public IResult GetLogs(int resultsPage, ISearchParameters parameters)
		{
			return new ResultLoader(_connection, resultsPage, parameters);
		}

		public void Connect(string settings)
		{
			_connection = new Database(settings, "System.Data.SqlClient");
		}
	}
}
