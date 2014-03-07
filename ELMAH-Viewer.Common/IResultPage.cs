using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public interface IResultPage
	{
		int Page { get; }
		IEnumerable<ISimpleErrorLog> Results { get; }
	}
}
