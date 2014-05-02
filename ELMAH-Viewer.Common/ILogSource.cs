using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMAH_Viewer.Common
{
	public interface ILogSource
	{
		// Use ISet to ensure no duplicates
		// ReSharper disable ReturnTypeCanBeEnumerable.Global
		ISet<string> Applications { get; }
		ISet<string> Hosts { get; }
		ISet<string> Types { get; }
		ISet<string> Sources { get; }
		ISet<string> Users { get; }
		ISet<int> StatusCodes { get; }
		// ReSharper restore ReturnTypeCanBeEnumerable.Global

		void Connect(string settings);
		IConnectionDialog GetConnectionDialog();
		void LoadSearchValues();
		Task<IErrorLog> GetLog(Guid errorId);
		IResult GetLogs(int resultsPage);
		IResult GetLogs(int resultsPage, ISearchParameters parameters);
		void DeleteLogs(ISearchParameters parameters);
	}
}
