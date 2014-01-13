using System;

namespace ELMAH_Viewer.Common
{
	public interface ISimpleErrorLog
	{
		Guid ErrorId { get; }
		string Application { get; }
		string Host { get; }
		string Type { get; }
		string Source { get; }
		string User { get; }
		int StatusCode { get; }
		DateTime TimeUtc { get; }
	}
}
