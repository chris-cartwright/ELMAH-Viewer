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

		public static bool IsTrue(this string val)
		{
			val = val.ToLower();
			return val == "true" || val == "t" || val == "1" || val == "on" || val == "y" || val == "yes";
		}

		public static bool IsFalse(this string val)
		{
			val = val.ToLower();
			return val == "false" || val == "f" || val == "0" || val == "off" || val == "n" || val == "no";
		}
	}
}
