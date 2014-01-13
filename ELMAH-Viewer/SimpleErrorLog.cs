using System;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer
{
	public class SimpleErrorLog : ISimpleErrorLog
	{
		public Guid ErrorId
		{
			get { return new Guid(); }
		}

		public string Application
		{
			get { return "Application"; }
		}

		public string Host {
			get { return "localhost:5056"; }
		}

		public string Type {
			get { return "System.Exception.NullReferenceException"; }
		}

		public string Source {
			get { return "Source"; }
		}

		public string User {
			get { return "chris.cartwright"; }
		}

		public int StatusCode {
			get { return 500; }
		}

		public DateTime TimeUtc {
			get { return DateTime.UtcNow; }
		}
	}
}
