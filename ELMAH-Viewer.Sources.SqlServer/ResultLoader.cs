using System.Collections;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultLoader : IResult
	{
		private readonly Database _connection;
		private readonly string _query;
		private int _page;

		public int ResultsPerPage { get; private set; }
		public IResultPage Current { get; private set; }

		object IEnumerator.Current
		{
			get { return Current; }
		}

		public int TotalResults { get; private set; }

		public ResultLoader(Database connection, int resultsPage)
		{
			_connection = connection;
			ResultsPerPage = resultsPage;
			_query = "SELECT * FROM ELMAH_Error ORDER BY TimeUtc DESC";
			_page = 1;
		}

		public void Dispose() { }

		public bool MoveNext()
		{
			Page<Elmah> next = _connection.Page<Elmah>(_page, ResultsPerPage, _query);
			_page++;
			if (next.Items.Count == 0)
			{
				return false;
			}

			Current = new ResultPage(next);
			return true;
		}

		public void Reset()
		{
			_page = 0;
			MoveNext();
		}
	}
}
