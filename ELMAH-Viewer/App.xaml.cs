using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace ELMAH_Viewer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(App));

		private static readonly string LogDatabasePath;

		static App()
		{
			LogDatabasePath = Path.Combine(Helpers.ApplicationPath, "ErrorLog.db");
		}

		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => Logger.Fatal(args.ExceptionObject);

			if (!File.Exists(LogDatabasePath))
			{
				Stream reader = Assembly.GetExecutingAssembly().GetManifestResourceStream("ELMAH_Viewer.Resources.ErrorLog.db");
				if (reader == null)
				{
					throw new ApplicationException("Missing embedded resource");
				}

				using (FileStream writer = new FileStream(LogDatabasePath, FileMode.Create))
				{
					byte[] buffer = new byte[1024];
					int read = 0;

					do
					{
						writer.Write(buffer, 0, read);
						read = reader.Read(buffer, 0, 1024);
					} while (read > 0);
				}
			}

			XmlConfigurator.Configure();
			Logger.Info("Starting application.");
		}
	}
}
