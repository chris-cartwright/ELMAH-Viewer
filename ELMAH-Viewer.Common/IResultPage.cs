using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public interface IResultPage
	{
		bool HasItems { get; }
		int Page { get; }
		IEnumerable<ISimpleErrorLog> Items { get; }
	}
}
