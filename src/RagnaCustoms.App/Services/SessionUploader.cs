using RagnaCustoms.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RagnaCustoms.Services
{
    public class SessionUploader
    {
        protected virtual Configuration Configuration { get; }
        protected virtual Uri Uri { get; }

        public SessionUploader(Configuration configuration, string uriStr)
        {
            Configuration = configuration;
            Uri = new Uri(uriStr);
        }

        public virtual async Task UploadAsync(string apiKey, params Session[] sessions)
        {
            if (!Configuration.SendScoreAutomatically) return;
            if (string.IsNullOrEmpty(apiKey)) return;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);

            var models = sessions.Select(session => new SessionModel(session.Song.Hash, session.Score, session.Song.Level)).ToArray();

            var result = await client.PostAsJsonAsync(Uri, models);
            if (result.IsSuccessStatusCode)
            {
                // TODO: Do something with the result
            }
            else
            {
                // TODO: Do something with the result
            }
        }
    }

    public record SessionModel(string HashInfo, string Score, string Level);
}
