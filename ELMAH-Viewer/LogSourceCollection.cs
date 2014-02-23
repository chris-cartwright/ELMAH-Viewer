using System;
using System.Collections.Generic;
using ELMAH_Viewer.Annotations;
using ELMAH_Viewer.Common;

namespace ELMAH_Viewer
{
	[UsedImplicitly]
	public class LogSourceCollection : List<Lazy<ILogSource, ILogSourceMetadata>>
	{
		public Lazy<ILogSource, ILogSourceMetadata> this[string guid]
		{
			get { return this.KeyLookup(s => s.Metadata.Guid == guid); }
		}
	}
}
