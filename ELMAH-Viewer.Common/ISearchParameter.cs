using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public interface ISearchParameter<T> : ICollection<T>
	{
		SearchMode Mode { get; }
	}
}
