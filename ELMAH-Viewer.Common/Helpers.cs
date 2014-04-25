using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

		public static string GetName<T>(this Expression<Func<T>> member)
		{
			MemberExpression memberExp = member.Body as MemberExpression;
			if (memberExp == null)
			{
				throw new ArgumentException("Expected member expression.", "member");
			}

			return memberExp.Member.Name;
		}
	}
}
