using System.Collections.Generic;

namespace ELMAH_Viewer.Common
{
	public static class Helpers
	{
		public static void AddRange<T>(this ICollection<T> dest, IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				dest.Add(item);
			}
		}
	}
}
