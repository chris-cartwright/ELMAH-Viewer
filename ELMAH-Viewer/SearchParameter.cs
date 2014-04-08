using System.Collections.Generic;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer
{
	public class SearchParameter<T> : List<T>, ISearchParameter<T>
	{
		public SearchMode Mode { get; set; }
	}
}
