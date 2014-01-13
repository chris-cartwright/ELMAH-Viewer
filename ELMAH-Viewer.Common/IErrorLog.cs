namespace ELMAH_Viewer.Common
{
	public interface IErrorLog : ISimpleErrorLog
	{
		string Message { get; }
		int Sequence { get; }
		string AllXml { get; }
	}
}
