using System.Configuration;

namespace Donateurs.Models
{
    public class ConfigurationSettings
    {
        public string Language { get; set; }
        public bool AutoRestart { get; set; }

        public static ConfigurationSettings Load()
        {
            return new ConfigurationSettings
            {
                Language = ConfigurationManager.AppSettings["Language"] ?? "fr",
                AutoRestart = bool.TryParse(ConfigurationManager.AppSettings["AutoRestart"], out var result) && result
            };
        }

        public void Save()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Clear();
            config.AppSettings.Settings.Add("Language", Language);
            config.AppSettings.Settings.Add("AutoRestart", AutoRestart.ToString());
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
