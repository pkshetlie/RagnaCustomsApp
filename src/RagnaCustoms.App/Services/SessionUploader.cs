using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using RagnaCustoms.Models;

namespace RagnaCustoms.Services
{
    public class SessionUploader
    {
        public SessionUploader(Configuration configuration, string uriStr)
        {
            Configuration = configuration;
            Uri = new Uri(uriStr);
        }

        protected virtual Configuration Configuration { get; }
        protected virtual Uri Uri { get; }

        public virtual async Task UploadAsync(Session session)
        {
            try
            {
                if (!Configuration.SendScoreAutomatically) return;
                if (string.IsNullOrEmpty(Configuration.ApiKey)) return;

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", Configuration.ApiKey);

                var models = new SessionModel(session);

                var result = await client.PostAsJsonAsync(Uri, models);
                if (result.IsSuccessStatusCode)
                    Trace.WriteLine(
                        $"{DateTime.Now} - Upload success - Hash: {session.Song.Hash}; Score: {session.Score}");
                else
                    Trace.WriteLine(
                        $"{DateTime.Now} - Upload error ({result.ReasonPhrase}) - Hash: {session.Song.Hash}; Score: {session.Score}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{DateTime.Now} - Upload error (Exception) - exception: {ex.Message}");
            }
        }
    }

    public class SessionModel
    {
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
    }
}