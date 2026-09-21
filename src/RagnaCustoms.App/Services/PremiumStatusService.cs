using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RagnaCustoms.Services
{
    public enum PremiumStatus
    {
        Unknown,
        NotAuthenticated,
        Checking,
        Premium,
        NotPremium,
        Unavailable
    }

    public sealed class PremiumStatusService
    {
        private const string AccountEndpoint = "https://api.ragnacustoms.com/api/account/me";

        public async Task<PremiumStatus> GetStatusAsync(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return PremiumStatus.NotAuthenticated;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
                    using (var response = await client.GetAsync(AccountEndpoint))
                    {
                        if (response.StatusCode == HttpStatusCode.Unauthorized ||
                            response.StatusCode == HttpStatusCode.Forbidden)
                        {
                            return PremiumStatus.NotPremium;
                        }

                        if (!response.IsSuccessStatusCode)
                        {
                            return PremiumStatus.Unavailable;
                        }

                        var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                        var isPremium = payload["isPremium"];
                        if (isPremium == null || isPremium.Type != JTokenType.Boolean)
                        {
                            return PremiumStatus.Unavailable;
                        }

                        return isPremium.Value<bool>()
                            ? PremiumStatus.Premium
                            : PremiumStatus.NotPremium;
                    }
                }
            }
            catch (Exception)
            {
                return PremiumStatus.Unavailable;
            }
        }
    }
}
