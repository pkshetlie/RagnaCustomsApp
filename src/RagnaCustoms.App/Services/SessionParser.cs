using RagnaCustoms.Models;
using System;
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
                    }
                    else if (line.Contains(songScoreLineHint))
                    {
                        var startIndex = line.IndexOf(songScoreLineHint) + songScoreLineHint.Length;
                        var endIndex = line.IndexOf("and adjusted distance =");

                        session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(new[] { ' ', '.' });

                        if (session.Song.Hash is null) continue;

                        OnNewSession?.Invoke(session);
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
