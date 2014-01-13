using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public interface ILogSource
	{
		ISet<string> Applications { get; }
		ISet<string> Hosts { get; }
		ISet<string> Types { get; }
		ISet<string> Sources { get; }
		ISet<string> Users { get; }
		ISet<int> StatusCodes { get; }

		ISimpleErrorLog[] GetLogs(int count, int offset);
		ISimpleErrorLog[] SearchLogs(int count, int offset, ISearchParameters parameters);
	}
}
