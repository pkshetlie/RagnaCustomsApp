using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediaDevices;
using RagnaCustoms.Models;

namespace RagnaCustoms.Services
{
    public class SessionParser
    {
        private const int WaitForNewContentMilliseconds = 1000;

        public SessionParser(string songLogFilePath)
        {
            IsRunning = false;
            SongLogFilePath = songLogFilePath;
        }

        protected virtual bool IsRunning { get; set; }
        protected virtual string SongLogFilePath { get; }
        protected virtual Task RunningTask { get; set; }

        public event Action<Session> OnNewSession;

        //public void StartAsync(MediaDevice device = null)
        //{
        //    RunningTask = Task.Run(() =>
        //    {
        //        IsRunning = true;
        //        string questSongDirectoryPath = null;

        //        if (device != null)
        //        {
        //            var base_folder = device.GetDirectories(@"\")[0];
        //            questSongDirectoryPath = $"{base_folder}{Oculus.QuestSongDirectoryName}";
        //        }

        //        var file = new FileInfo(SongLogFilePath);
        //        var length = file.Length;

        //        using var stream = File.Open(SongLogFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //        using var reader = new StreamReader(stream);

        //        var session = new Session();

        //        while (IsRunning)
        //        {
        //            var line = reader.ReadLine();

        //            var songLevelLineHint = "LogTemp: Warning: Song level str";
        //            var songNameLineHint = "LogTemp: Loading song";
        //            var songScoreDetailLineHint = "LogTemp: Warning: Notes missed :";
        //            var songScoreLineHint = "raw distance =";

        //            if (line.Contains(songLevelLineHint))
        //            {
        //                session = new Session();
        //                session.Song.Level = line.Substring(line.IndexOf(songLevelLineHint) + songLevelLineHint.Length)
        //                    .Trim(' ', '.');
        //            }
        //            else if (line.Contains(songScoreDetailLineHint))
        //            {
        //                //        LogTemp: Warning: Notes missed : 0 / Notes hit  : 784 / Notes not processed : 0 / hit accuracy : 0.015131 / percentage : 0.848690 / hit speed : 1077.700073 / percentage : 0.698357 / combos : 7
        //                var startIndex = line.IndexOf(songScoreDetailLineHint) + songScoreDetailLineHint.Length;
        //                var endIndex = line.IndexOf(" / Notes hit  : ");
        //                session.NotesMissed =
        //                    int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

        //                startIndex = line.IndexOf(" / Notes hit  : ") + " / Notes hit  : ".Length;
        //                endIndex = line.IndexOf(" / Notes not processed : ");
        //                session.NotesHit = int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

        //                startIndex = line.IndexOf(" / Notes not processed : ") + " / Notes not processed : ".Length;
        //                endIndex = line.IndexOf(" / hit accuracy : ");
        //                session.NotesNotProcessed =
        //                    int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

        //                startIndex = line.IndexOf(" / hit accuracy : ") + " / hit accuracy : ".Length;
        //                endIndex = line.IndexOf(" / percentage : ");
        //                session.HitAccuracy = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                startIndex = line.IndexOf(" / hit accuracy : ") + " / hit accuracy : ".Length;
        //                endIndex = startIndex + 9;
        //                session.HitAccuracy = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                startIndex = endIndex + 14;
        //                endIndex = startIndex + 9;
        //                session.Percentage = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                startIndex = endIndex + 14;
        //                endIndex = startIndex + 12;
        //                session.HitSpeed = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                startIndex = endIndex + 15;
        //                endIndex = startIndex + 8;
        //                session.Percentage2 = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                startIndex = endIndex + 12;
        //                endIndex = line.Length;
        //                var content = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');
        //                session.Combos = int.Parse(content);
        //            }
        //            else if (line.Contains(songNameLineHint))
        //            {
        //                var songOggPath = line.Substring(line.IndexOf(songNameLineHint) + songNameLineHint.Length)
        //                    .Trim(' ', '.');
        //                var songDirectoryPath = Path.GetDirectoryName(songOggPath);

        //                // if log file is coming from quest, we need to download song before calculating hash file
        //                if (device != null)
        //                {
        //                    var songName = songDirectoryPath.Split('\\').Last();
        //                    var tempDir = Path.GetDirectoryName(SongLogFilePath);
        //                    var tempSongFolder = tempDir + "\\" + songName;
        //                    var oculusSongFolder = questSongDirectoryPath + "\\" + songName;
        //                    if (!device.DirectoryExists(oculusSongFolder)) continue;
        //                    device.DownloadFolder(oculusSongFolder, tempSongFolder);
        //                    songDirectoryPath = tempSongFolder;
        //                }

        //                var songDirectory = new DirectoryInfo(songDirectoryPath);
        //                if (!songDirectory.Exists) continue;

        //                var songDatFiles = songDirectory.EnumerateFiles("*.dat");

        //                var filesHashs = songDatFiles.Select(ComputeMd5).OrderBy(hash => hash);
        //                var concatenatedHashs = string.Concat(filesHashs);

        //                session.Song.Hash = ComputeMd5(concatenatedHashs);
        //                Trace.WriteLine(
        //                    $"{DateTime.Now} - New file - Hash: {session.Song.Hash}; File: {songDirectoryPath}");
        //            }
        //            else if (line.Contains(songScoreLineHint))
        //            {
        //                var startIndex = line.IndexOf(songScoreLineHint) + songScoreLineHint.Length;
        //                var endIndex = line.IndexOf("and adjusted distance =");

        //                session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

        //                if (session.Song.Hash is null) continue;
        //                Trace.WriteLine(
        //                    $"{DateTime.Now} - New session - Hash: {session.Song.Hash}; Level: {session.Song.Level}; Score: {session.Score}");
        //                OnNewSession?.Invoke(session);
        //                line = reader.ReadLine();
        //            }

        //            while (reader.EndOfStream)
        //            {
        //                file = new FileInfo(SongLogFilePath);

        //                if (file.Length < length)
        //                {
        //                    stream.Seek(0, SeekOrigin.Begin);
        //                    reader.DiscardBufferedData();
        //                }

        //                length = file.Length;

        //                Thread.Sleep(WaitForNewContentMilliseconds);

        //                // there will be no new data once the end of file is reach for oculus log
        //                if (device != null)
        //                {
        //                    IsRunning = false;
        //                    return;
        //                }
        //            }
        //        }
        //    });

        //    // Waiting for oculus send score to finish before continuig and thus removing temp folder
        //    if (device != null) RunningTask.Wait();
        //}

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