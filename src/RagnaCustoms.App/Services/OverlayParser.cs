using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RagnaCustoms.Models;

namespace RagnaCustoms.Services
{
    public class OverlayParser
    {
        private const int WaitForNewContentMilliseconds = 1000;

        public OverlayParser(string songLogFilePath)
        {
            IsRunning = false;
            SongLogFilePath = songLogFilePath;
        }

        protected virtual bool IsRunning { get; set; }
        protected virtual string SongLogFilePath { get; }
        protected virtual Task RunningTask { get; set; }

        public event Action<Session> OnOverlayNewGame;
        public event Action<Session> OnOverlayStartGame;
        public event Action<Session> OnOverlayEndGame;

        public void StartAsync()
        {
            RunningTask = Task.Run(() =>
            {
                IsRunning = true;

                var file = new FileInfo(SongLogFilePath);
                if (!file.Exists)
                {
                    var fullDir = Path.GetDirectoryName(SongLogFilePath);
                    if (!Directory.Exists(fullDir))
                    {
                        Directory.CreateDirectory(fullDir);
                    }
                    var tempFile = File.Create(SongLogFilePath);
                    tempFile.Close();
                }
                var length = file.Length;

                using var stream = File.Open(SongLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                var session = new Session();

                while (IsRunning)
                {
                    var line = reader.ReadLine();
                    if (line == null || line.Length == 0) 
                    {
                        continue;
                    }
                    var songLevelLineHint = "LogTemp: Warning: Song level str";
                    var songNameLineHint = "LogTemp: Loading song";
                    var songScoreLineHint = "raw distance =";

                    if (line.Contains(songLevelLineHint))
                    {
                        session = new Session();
                        try
                        {
                            OnOverlayNewGame?.Invoke(session);
                        }
                        catch (Exception ex)
                        {
                        }

                        session.Song.Level = line.Substring(line.IndexOf(songLevelLineHint) + songLevelLineHint.Length)
                            .Trim(' ', '.');
                    }
                    else if (line.Contains(songNameLineHint))
                    {
                        var songOggPath = line.Substring(line.IndexOf(songNameLineHint) + songNameLineHint.Length)
                            .Trim(' ', '.');

                        var songDirectoryPath = Path.GetDirectoryName(songOggPath);
                        var songDirectory = new DirectoryInfo(songDirectoryPath);
                        if (!songDirectory.Exists) continue;

                        var songDatFiles = songDirectory.EnumerateFiles("*.dat");

                        var filesHashs = songDatFiles.Select(ComputeMd5).OrderBy(hash => hash);
                        var concatenatedHashs = string.Concat(filesHashs);

                        session.Song.Hash = ComputeMd5(concatenatedHashs);
                        var overlaySession = new SessionModel(session.Song.Hash, "0", session.Song.Level);
                        try
                        {
                            OnOverlayStartGame?.Invoke(session);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (line.Contains(songScoreLineHint))
                    {
                        session.Song.Hash = "EndOfSong";
                        try
                        {
                            OnOverlayEndGame?.Invoke(session);
                        }
                        catch (Exception ex)
                        {
                        }
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

        public virtual void Stop()
        {
            IsRunning = false;
            RunningTask.Wait();
        }

        protected virtual string ComputeMd5(FileInfo file)
        {
            using var md5 = MD5.Create();
            using var stream = file.OpenRead();

            var hash = md5.ComputeHash(stream);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }

        protected virtual string ComputeMd5(string str)
        {
            using var md5 = MD5.Create();

            var strBytes = Encoding.Default.GetBytes(str);
            var hash = md5.ComputeHash(strBytes);
            var hashStr = BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();

            return hashStr;
        }
    }
}