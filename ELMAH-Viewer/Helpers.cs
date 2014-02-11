using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using ELMAH_Viewer.Annotations;

namespace ELMAH_Viewer
{
	public static class Helpers
	{
		// http://stackoverflow.com/a/283917
		public static string ApplicationPath
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}

		public static IEnumerable<T> FindVisualChildren<T>([NotNull] this DependencyObject source) where T : DependencyObject
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(source); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(source, i);
				if (child is T)
				{
					yield return child as T;
				}

				foreach (T item in FindVisualChildren<T>(child))
				{
					yield return item;
				}
			}
		}
	}
}
