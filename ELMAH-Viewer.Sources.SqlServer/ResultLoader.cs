using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultLoader : IResult
	{
		private static void AddParameter<T>(Sql sql, ISearchParameter<T> param)
		{
			if (param.Count > 0)
			{
				sql.Where("Application in (@test)", new { test = param.ToArray() });
			}
		}

		private readonly Database _connection;
		private readonly Sql _query;

		public int ResultsPerPage { get; private set; }
		public long TotalResults { get; private set; }

		public ResultLoader(Database connection, int resultsPage)
		{
			_connection = connection;
			ResultsPerPage = resultsPage;
			_query = Sql.Builder.Append("SELECT * FROM ELMAH_Error ORDER BY TimeUtc DESC");
		}

		public ResultLoader(Database connection, int resultsPage, ISearchParameters parameters)
			: this(connection, resultsPage)
		{
			_query = Sql.Builder.Select("*").From("ELMAH_Error");

			AddParameter(_query, parameters.Application);
			AddParameter(_query, parameters.Host);
			AddParameter(_query, parameters.Type);
			AddParameter(_query, parameters.Source);
			AddParameter(_query, parameters.User);
			AddParameter(_query, parameters.StatusCode);
			AddParameter(_query, parameters.Severity);

			if (parameters.BeginTimeStamp != null)
			{
				_query.Where("TimeUtc>=@0", parameters.BeginTimeStamp);
			}

			if (parameters.EndTimeStamp != null)
			{
				_query.Where("TimeUtc<=@0", parameters.EndTimeStamp);
			}
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
