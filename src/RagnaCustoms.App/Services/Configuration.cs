using System.Configuration;
using System.Windows.Forms;

namespace RagnaCustoms.Services
{
    public class Configuration
    {
        public string ApiKey
        {
            get => Get(nameof(ApiKey));
            set => Set(nameof(ApiKey), value);
        }

        public string TwitchChannel
        {
            get => Get(nameof(TwitchChannel));
            set => Set(nameof(TwitchChannel), value);
        }

        public string AuthTmi
        {
            get => Get(nameof(AuthTmi));
            set => Set(nameof(AuthTmi), value);
        }

        public bool SendScoreAutomatically
        {
            get => Get(nameof(SendScoreAutomatically)) == bool.TrueString;
            set => Set(nameof(SendScoreAutomatically), value ? bool.TrueString : bool.FalseString);     
        } 
        
        public bool AutoCloseDownload
        {
            get => Get(nameof(AutoCloseDownload)) == bool.TrueString;
            set => Set(nameof(AutoCloseDownload), value ? bool.TrueString : bool.FalseString); 
        }
        public string BotPrefix {
            get => Get(nameof(BotPrefix));
            set => Set(nameof(BotPrefix), value);
        }

        private string Get(string key)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            return config.AppSettings.Settings[key].Value;
        }
        private void Set(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
