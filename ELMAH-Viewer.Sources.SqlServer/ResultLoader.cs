using System.Threading.Tasks;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultLoader : IResult
	{
		private readonly Database _connection;
		private readonly string _query;

		public int ResultsPerPage { get; private set; }
		public long TotalResults { get; private set; }

		public ResultLoader(Database connection, int resultsPage)
		{
			_connection = connection;
			ResultsPerPage = resultsPage;
			_query = "SELECT * FROM ELMAH_Error ORDER BY TimeUtc DESC";
		}

		public async Task<IResultPage> GetPageAsync(int page)
		{
			return await Task.Factory.StartNew<IResultPage>(() =>
			{
				Page<Elmah> next = _connection.Page<Elmah>(page, ResultsPerPage, _query);
				if (next.Items.Count == 0)
				{
					return new ResultPage();
				}

				TotalResults = next.TotalItems;
				return new ResultPage(next);
			});
		}
	}
}
