using System.Threading.Tasks;

namespace ELMAH_Viewer.Common
{
	public interface IResult
	{
		int ResultsPerPage { get; }
		long TotalResults { get; }
		Task<IResultPage> GetPageAsync(int page);
	}
}
