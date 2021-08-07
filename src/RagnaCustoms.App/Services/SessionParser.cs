using Newtonsoft.Json;
using RagnaCustoms.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RagnaCustoms.Services
{
    public class SessionParser
    {
        const int WaitForNewContentMilliseconds = 1000;

        public event Action<Session> OnNewSession;

        protected virtual bool IsRunning { get; set; }
        protected virtual string SongLogFilePath { get; }
        protected virtual Task RunningTask { get; set; }

        protected string OverlayPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "overlay.html");
        public SessionParser(string songLogFilePath)
        {
            IsRunning = false;
            SongLogFilePath = songLogFilePath;
        }

        public void StartAsync()
        {
            RunningTask = Task.Run(() =>
            {
                IsRunning = true;

                var file = new FileInfo(SongLogFilePath);
                var length = file.Length;

                using var stream = File.Open(SongLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                var session = new Session();

                while (IsRunning)
                {
                    var line = reader.ReadLine();

                    var songLevelLineHint = "LogTemp: Warning: Song level str";
                    var songNameLineHint = "LogTemp: Loading song";
                    var songScoreLineHint = "raw distance =";

                    if (line.Contains(songLevelLineHint))
                    {
                        ResetOverlay();
                        session = new Session();
                        session.Song.Level = line.Substring(line.IndexOf(songLevelLineHint) + songLevelLineHint.Length).Trim(new[] { ' ', '.' });
                    }
                    else if (line.Contains(songNameLineHint))
                    {
                        var songOggPath = line.Substring(line.IndexOf(songNameLineHint) + songNameLineHint.Length).Trim(new[] { ' ', '.' });

                        var songDirectoryPath = Path.GetDirectoryName(songOggPath);
                        var songDirectory = new DirectoryInfo(songDirectoryPath);
                        if (!songDirectory.Exists) continue;

                        var songDatFiles = songDirectory.EnumerateFiles("*.dat");

                        var filesHashs = songDatFiles.Select(ComputeMD5).OrderBy(hash => hash);
                        var concatenatedHashs = string.Concat(filesHashs);

                        session.Song.Hash = ComputeMD5(concatenatedHashs);
                        Trace.WriteLine($"{DateTime.Now} - New file - Hash: {session.Song.Hash}; File: {songDirectoryPath}");

                        var infoFile = songDirectory.GetFiles().FirstOrDefault(x => x.FullName.ToLower().Contains("info.dat"));
                        var infoText = File.ReadAllText(infoFile.FullName);
                        var info = JsonConvert.DeserializeObject<InfoDat>(infoText);

                        File.WriteAllText(OverlayPath,
                            "<html>" +
                            "<head>" +
                            "<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css' integrity='sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l' crossorigin='anonymous'>" +
                            "<style>" +
                            "html,body{background:transparent;}" +

                            "h1,h3,h4{" +
                            "	color: #ffffff;" +
                            "	text-shadow: 0 0 10px #000000;" +
                            "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +
                            "	<div class='d-flex'>" +
                            "	<div><img class='pr-3' style='width:160px;' src='" + Path.Combine(songDirectoryPath, info._coverImageFilename).ToString() + "'></div>" +
                            "	<div><h1>" + info._songName + " level " + session.Song.Level + "</h1>" +
                            "	<h3>" + info._songAuthorName + "</h3>" +
                            "	<h4>mapped by : " + info._levelAuthorName + "</h4>" +
                            "	</div>" +
                            "	</div>" +
                            "	<script>" +
                            "	setTimeout(function(){window.location.reload();},5000);" +
                            "</script>" +
                            "</body>" +
                            "</html>"

                            ); ;
                    }
                    else if (line.Contains(songScoreLineHint))
                    {
                        var startIndex = line.IndexOf(songScoreLineHint) + songScoreLineHint.Length;
                        var endIndex = line.IndexOf("and adjusted distance =");

                        session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(new[] { ' ', '.' });

                        if (session.Song.Hash is null) continue;
                        Trace.WriteLine($"{DateTime.Now} - New session - Hash: {session.Song.Hash}; Level: {session.Song.Level}; Score: {session.Score}");
                        OnNewSession?.Invoke(session);
                        line = reader.ReadLine();
                        ResetOverlay();
                    }

                    while (reader.EndOfStream)
                    {
                        file = new FileInfo(SongLogFilePath);

                        if (file.Length < length)
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                            reader.DiscardBufferedData();
                        }

                        length = file.Length;

                        Thread.Sleep(WaitForNewContentMilliseconds);
                    }
                }
            });
        }

        private void ResetOverlay()
        {
            File.WriteAllText(OverlayPath,
                            "<html>" +
                            "<head>" +
                            "<link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/css/bootstrap.min.css' integrity='sha384-B0vP5xmATw1+K9KRQjQERJvTumQW0nPEzvF6L/Z6nronJ3oUOFUFpCjEUQouq2+l' crossorigin='anonymous'>" +
                            "<style>" +
                            "h1,h3,h4{" +
                            "	color: #ffffff;" +
                            "	text-shadow: 0 0 10px #000000;" +
                            "}" +
                            "</style>" +
                            "</head>" +
                            "<body>" +
                            "	<script>" +
                            "	setTimeout(function(){window.location.reload();},5000);" +
                            "</script>" +
                            "</body>" +
                            "</html>");

        }

        public virtual void Stop()
        {
            IsRunning = false;
            RunningTask.Wait();
        }

        protected virtual string ComputeMD5(FileInfo file)
        {
            using var md5 = MD5.Create();
            using var stream = file.OpenRead();

            var hash = md5.ComputeHash(stream);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }

        protected virtual string ComputeMD5(string str)
        {
            using var md5 = MD5.Create();

            var strBytes = Encoding.Default.GetBytes(str);
            var hash = md5.ComputeHash(strBytes);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }
    }
}
