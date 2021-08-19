using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace RagnaCustoms.Services
{
    public class Configuration
    {

        const string UserRoot = "HKEY_CURRENT_USER";
        const string Subkey = "Software\\RagnaCustoms\\RagnaCustoms";
        const string KeyName = UserRoot + "\\" + Subkey;

        public string Lang
        {
            get => Get(nameof(Lang));
            set => Set(nameof(Lang), value);
        }

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
        
        public bool Overlay
        {
            get => Get(nameof(Overlay)) == bool.TrueString;
            set => Set(nameof(Overlay), value ? bool.TrueString : bool.FalseString);     
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

        public Dictionary<string, string> ViewerLang
        {
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(Get(nameof(ViewerLang)));
            }
            set
            {
                Set(nameof(ViewerLang), JsonConvert.SerializeObject(value));
            }
        }

        private string Get(string key)
        {
           return (string) Registry.GetValue(KeyName,
            key,
            "");
        }
        private void Set(string key, string value)
        {
            Registry.SetValue(KeyName, key, value);
        }
    }
}
