using System;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer
{
	public class SearchParameters : ISearchParameters
	{
		public SearchParameter<string> Application { get; set; }
		public SearchParameter<string> Host { get; set; }
		public SearchParameter<string> Type { get; set; }
		public SearchParameter<string> Source { get; set; }
		public SearchParameter<string> User { get; set; }
		public SearchParameter<int> StatusCode { get; set; }
		public SearchParameter<byte> Severity { get; set; }
		public DateTime BeginTimeStamp { get; set; }
		public DateTime EndTimeStamp { get; set; }

		ISearchParameter<string> ISearchParameters.Application
		{
			get { return Application; }
		}

		ISearchParameter<string> ISearchParameters.Host
		{
			get { return Host; }
		}

		ISearchParameter<string> ISearchParameters.Type
		{
			get { return Type; }
		}

		ISearchParameter<string> ISearchParameters.Source
		{
			get { return Source; }
		}

		ISearchParameter<string> ISearchParameters.User
		{
			get { return User; }
		}

		ISearchParameter<int> ISearchParameters.StatusCode
		{
			get { return StatusCode; }
		}

		ISearchParameter<byte> ISearchParameters.Severity
		{
			get { return Severity; }
		}

		public SearchParameters()
		{
			Application = new SearchParameter<string>();
			Host = new SearchParameter<string>();
			Type = new SearchParameter<string>();
			Source = new SearchParameter<string>();
			User = new SearchParameter<string>();
			StatusCode = new SearchParameter<int>();
			Severity = new SearchParameter<byte>();
		}
	}
}
