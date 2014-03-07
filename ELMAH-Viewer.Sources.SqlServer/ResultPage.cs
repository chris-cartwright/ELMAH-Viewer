using System.Collections.Generic;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultPage : IResultPage
	{
		public int Page { get; private set; }
		public IEnumerable<ISimpleErrorLog> Results { get; private set; }

		public ResultPage(Page<Elmah> page)
		{
			Page = (int)page.CurrentPage;

			Results = page.Items;
		}
	}
}
