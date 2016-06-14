using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultHandler : IResult
	{
		private static void AddParameter<T>(Sql sql, Expression<Func<ISearchParameter<T>>> param)
		{
			ISearchParameter<T> p = param.Compile()();
			if (p.Count == 0 || p.Mode == SearchMode.None)
			{
				return;
			}

			if (p.Mode == SearchMode.Inclusive)
			{
				sql.Where("[" + param.GetName() + "] in (@test)", new { test = p.ToArray() });
			}
			else
			{
				sql.Where("[" + param.GetName() + "] not in (@test)", new { test = p.ToArray() });
			}
		}

		private readonly Database _connection;
		private readonly ISearchParameters _params;

		public int ResultsPerPage { get; private set; }
		public long TotalResults { get; private set; }

		private void AddWhere(Sql query)
		{
			if (_params == null)
			{
				return;
			}

			AddParameter(query, () => _params.Application);
			AddParameter(query, () => _params.Host);
			AddParameter(query, () => _params.Type);
			AddParameter(query, () => _params.Source);
			AddParameter(query, () => _params.User);
			AddParameter(query, () => _params.StatusCode);
			AddParameter(query, () => _params.Severity);

			if (_params.BeginTimeStamp != null)
			{
				query.Where("[TimeUtc]>=@0", _params.BeginTimeStamp);
			}

			if (_params.EndTimeStamp != null)
			{
				query.Where("[TimeUtc]<=@0", _params.EndTimeStamp);
			}

		    if (_params.ErrorId != null)
		    {
		        query.Where("[ErrorId]=@0", _params.ErrorId);
		    }

		    if (!String.IsNullOrWhiteSpace(_params.Contains))
		    {
		        query.Where("[AllXml] LIKE @0", $"%{_params.Contains}%");
		    }
		}

		public ResultHandler(Database connection)
		{
			_connection = connection;
		}

		public ResultHandler(Database connection, int resultsPerPage)
			: this(connection)
		{
			ResultsPerPage = resultsPerPage;
		}

		public ResultHandler(Database connection, ISearchParameters parameters)
			: this(connection)
		{
			_params = parameters;
		}

		public ResultHandler(Database connection, int resultsPerPage, ISearchParameters parameters)
			: this(connection, parameters)
		{
			ResultsPerPage = resultsPerPage;
		}

		public async Task<IResultPage> GetPageAsync(int page)
		{
			return await Task.Factory.StartNew<IResultPage>(() =>
			{
				Sql query = Sql.Builder.Select("*").From("ELMAH_Error");
				AddWhere(query);
				query.OrderBy("TimeUtc DESC");

				Page<Elmah> next = _connection.Page<Elmah>(page, ResultsPerPage, query);
				if (next.Items.Count == 0)
				{
					return new ResultPage();
				}

				TotalResults = next.TotalItems;
				return new ResultPage(next);
			});
		}

		public void Delete()
		{
			Sql query = Sql.Builder.Append("DELETE").From("ELMAH_Error");
			AddWhere(query);
			_connection.Execute(query);
		}
	}
}
