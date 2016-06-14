using System;

namespace ELMAH_Viewer.Common
{
    public interface ISearchParameters
    {
        ISearchParameter<string> Application { get; }
        ISearchParameter<string> Host { get; }
        ISearchParameter<string> Type { get; }
        ISearchParameter<string> Source { get; }
        ISearchParameter<string> User { get; }
        ISearchParameter<int> StatusCode { get; }
        ISearchParameter<byte> Severity { get; }
        DateTime? BeginTimeStamp { get; }
        DateTime? EndTimeStamp { get; }
        Guid? ErrorId { get; }
        string Contains { get; }
    }
}
