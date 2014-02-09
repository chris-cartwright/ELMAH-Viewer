using System.Configuration;

namespace ELMAH_Viewer.Configuration
{
	public partial class SettingsSection
	{
		private static readonly System.Configuration.Configuration _configuration;

		public static SettingsSection Instance { get; private set; }

		static SettingsSection()
		{
			_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			Instance = (SettingsSection)_configuration.GetSection("settings");
		}

		public static void Save()
		{
			_configuration.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("settings");
		}
	}
}
