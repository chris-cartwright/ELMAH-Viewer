namespace ELMAH_Viewer.Sources.SqlServer.Converters
{
	public class InverseConverter : BooleanConverter<bool>
	{
		public InverseConverter() :
			base(false, true) { }
	}
}
