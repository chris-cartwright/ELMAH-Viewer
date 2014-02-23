using ELMAH_Viewer.Annotations;

namespace ELMAH_Viewer.Configuration
{
	[UsedImplicitly]
	public partial class SavedConnectionsCollection
	{
		public string this[string guid, string name]
		{
			get { return this.KeyLookup(c => c.Provider == guid && c.Name == name).Content; }
		}
	}
}
