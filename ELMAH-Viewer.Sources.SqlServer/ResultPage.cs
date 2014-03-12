using System.Collections.Generic;
using ELMAH_Viewer.Common;
using PetaPoco;

namespace ELMAH_Viewer.Sources.SqlServer
{
	public class ResultPage : IResultPage
	{
		public bool HasItems { get; private set; }
		public int Page { get; private set; }
		public IEnumerable<ISimpleErrorLog> Items { get; private set; }

		public ResultPage()
		{
			HasItems = false;
		}

		public ResultPage(Page<Elmah> page)
		{
			Page = (int)page.CurrentPage;
			HasItems = true;
			Items = page.Items;
		}
	}
}
