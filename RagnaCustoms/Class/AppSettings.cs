using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagnaCustoms.Class
{
    public class AppSettings : ApplicationSettingsBase
    {

        [UserScopedSetting()]
        [DefaultSettingValue("True")]
        public bool AutoClose
        {
            get
            {
                return (bool)this["AutoClose"];
            }
            set
            {
                this["AutoClose"] = value;
            }
        }


        [UserScopedSetting()]
        [DefaultSettingValue("en-US")]
        public string Culture
        {
            get
            {
                return (string)this["Culture"];
            }
            set
            {
                this["Culture"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool AutoDownload
        {
            get
            {
                return (bool)this["AutoDownload"];
            }
            set
            {
                this["AutoDownload"] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string ScoringApiKey
        {
            get
            {
                return (string)this["ScoringApiKey"];
            }
            set
            {
                this["ScoringApiKey"] = value;
            }
        } 
        
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string AuthTmi
        {
            get
            {
                return (string)this["AuthTmi"];
            }
            set
            {
                this["AuthTmi"] = value;
            }
        }
        [UserScopedSetting()]
        [DefaultSettingValue("False")]
        public bool TwitchBotEnabled{
            get
            {
                return (bool)this["TwitchBotEnabled"];
            }
            set
            {
                this["TwitchBotEnabled"] = value;
            }
        }
        
        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string TwitchChannel
        {
            get
            {
                return (string)this["TwitchChannel"];
            }
            set
            {
                this["TwitchChannel"] = value;
            }
        }

      
        public override void Save()
        {
            
            base.Save();
        }
    }
}
