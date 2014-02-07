using System.Configuration;
using System.Xml;

namespace ELMAH_Viewer.Configuration
{
	public partial class ConnectionElement
	{
		[ConfigurationProperty("content", IsRequired = false, IsKey = false, IsDefaultCollection = false)]
		public string Content
		{
			get { return (string)base["content"]; }
			set { base["content"] = value; }
		}

		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			Name = reader[NamePropertyName];
			Content = reader.ReadElementContentAsString();
		}

		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			// ReSharper claims this will never be null; running the code says otherwise
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			// ReSharper disable HeuristicUnreachableCode
			if (writer == null)
				return false;
			// ReSharper restore HeuristicUnreachableCode

			writer.WriteStartAttribute(NamePropertyName);
			writer.WriteString(Name);
			writer.WriteEndAttribute();

			writer.WriteCData(Content);

			return true;
		}
	}
}
