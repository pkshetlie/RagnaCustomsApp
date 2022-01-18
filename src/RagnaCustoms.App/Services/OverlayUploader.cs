using RagnaCustoms.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

namespace RagnaCustoms.Services
{
    public class OverlayUploader
    {
        protected virtual Configuration Configuration { get; }
        protected virtual Uri Uri { get; }

        public OverlayUploader(Configuration configuration, string uriStr)
        {
            Configuration = configuration;
            Uri = new Uri(uriStr);
        }

        public virtual async Task UploadAsync(string apiKey, params Session[] sessions)
        {
            try
            {
                if (!Configuration.Overlay) return;
                if (string.IsNullOrEmpty(apiKey)) return;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var models = sessions.Select(session => new SessionModel(session.Song.Hash, session.Score, session.Song.Level)).ToArray();

                var result = await client.PostAsJsonAsync(Uri, models);
                if (result.IsSuccessStatusCode)
                {
                    Trace.WriteLine($"{DateTime.Now} - Send to overlay success - Hash: {sessions[0].Song.Hash};");
                }
                else
                {
                    Trace.WriteLine($"{DateTime.Now} -  Send to overlay error ({result.ReasonPhrase}) - Hash: { sessions[0].Song.Hash};");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{DateTime.Now} - Upload error (Exception) - exception: { ex.Message }");
            }
        }
    }
}
