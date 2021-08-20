using RagnaCustoms.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

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

            var models = sessions.Select(session => new SessionModel(session)).ToArray();

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
        public string AppVersion { get; set; }

        public int NotesMissed { get; set; }
        public int NotesHit { get; set; }
        public int NotesNotProcessed { get; set; }
        public string HitAccuracy { get; set; }
        public string Percentage { get; set; }
        public string HitSpeed { get; set; }
        public string Percentage2 { get; set; }
        public int Combos { get; set; }

        public SessionModel(string hashInfo, string score, string level)
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            HashInfo = hashInfo;
            Score = score;
            Level = level;
        }

        public SessionModel(Session session)
        {

            AppVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            HashInfo = session.Song.Hash;
            Score = session.Score;
            Level = session.Song.Level;
            NotesMissed = session.NotesMissed;
            NotesHit = session.NotesHit;
            NotesNotProcessed = session.NotesNotProcessed;
            HitAccuracy = session.HitAccuracy;
            Percentage = session.Percentage;
            HitSpeed = session.HitSpeed;
            Percentage2 = session.Percentage2;
            Combos = session.Combos;
        }
    } 
}
