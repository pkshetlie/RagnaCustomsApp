using RagnaCustoms.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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
                Trace.WriteLine($"{DateTime.Now} - Upload success - Hash: {sessions[0].Song.Hash}; Score: {sessions[0].Score}");
            }
            else
            {
                Trace.WriteLine($"{DateTime.Now} - Upload error ({result.ReasonPhrase}) - Hash: { sessions[0].Song.Hash}; Score: {sessions[0].Score}");
            }
        }
    }

    public class SessionModel
    {
        public string HashInfo { get; set; }
        public string Score { get; set; }
        public string Level { get; set; }

        public SessionModel(string hashInfo, string score, string level)
        {
            HashInfo = hashInfo;
            Score = score;
            Level = level;
        }
    } 
}
