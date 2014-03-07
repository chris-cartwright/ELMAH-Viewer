using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public interface IResult : IEnumerator<IResultPage>
	{
		int TotalResults { get; }
	}
}
