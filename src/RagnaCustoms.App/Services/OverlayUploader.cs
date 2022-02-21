using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RagnaCustoms.App;
using RagnaCustoms.Models;

namespace RagnaCustoms.Services
{
    public class OverlayUploader
    {
        public OverlayUploader(Configuration configuration, string uriStr, string cleanUriStr)
        {
            Configuration = configuration;
            Uri = new Uri(uriStr);
            CleanUri = new Uri(cleanUriStr);
        }

        protected virtual Configuration Configuration { get; }
        protected virtual Uri Uri { get; }
        public virtual Uri CleanUri { get; }

        public virtual async Task UploadAsync(string apiKey, Session session)
        {
            try
            {
                if (!Configuration.Overlay) return;
                if (string.IsNullOrEmpty(apiKey)) return;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                var models = session;

                var result = await client.PostAsJsonAsync(Uri, models);
                if (result.IsSuccessStatusCode)
                    Trace.WriteLine($"{DateTime.Now} - Send to overlay success - Hash: {session.Song.Hash};");
                else
                    Trace.WriteLine(
                        $"{DateTime.Now} -  Send to overlay error ({result.ReasonPhrase}) - Hash: {session.Song.Hash};");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{DateTime.Now} - Upload error (Exception) - exception: {ex.Message}");
            }
        }
        
        public virtual async Task CleanAsync(string apiKey, Session session)
        {
            try
            {
                if (!Configuration.Overlay) return;
                if (string.IsNullOrEmpty(apiKey)) return;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);


                var result = await client.PostAsJsonAsync(CleanUri, new{ });
                if (result.IsSuccessStatusCode)
                    Trace.WriteLine($"{DateTime.Now} - Send clean to overlay success;");
                else
                    Trace.WriteLine(
                        $"{DateTime.Now} -  Send clean to overlay error ({result.ReasonPhrase}) ");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{DateTime.Now} - Clean overlay error (Exception) - exception: {ex.Message}");
            }
        }
    }
}