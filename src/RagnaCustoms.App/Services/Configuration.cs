using System.Configuration;

namespace RagnaCustoms.Services
{
    public class Configuration
    {
        public string ApiKey
        {
            get => Get(nameof(ApiKey));
            set => Set(nameof(ApiKey), value);
        }

        public bool SendScoreAutomatically
        {
            get => Get(nameof(SendScoreAutomatically)) == bool.TrueString;
            set => Set(nameof(SendScoreAutomatically), value ? bool.TrueString : bool.FalseString);
        }

        private string Get(string key) => ConfigurationManager.AppSettings[key];
        private void Set(string key, string value) => ConfigurationManager.AppSettings[key] = value;
    }
}
