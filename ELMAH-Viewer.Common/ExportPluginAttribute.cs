using System;
using System.ComponentModel.Composition;

namespace ELMAH_Viewer.Common
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ExportPluginAttribute : ExportAttribute, ILogSourceMetadata
	{
		public string Name { get; private set; }
		public string Guid { get; private set; }

		public ExportPluginAttribute(string name, string guid)
			: base(typeof(ILogSource))
		{
			Name = name;
			Guid = guid;
		}
	}
}
