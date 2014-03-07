using System.Collections.Generic;

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
		IResult GetLogs(int resultsPage);
		IResult GetLogs(int resultsPage, ISearchParameters parameters);
	}
}
