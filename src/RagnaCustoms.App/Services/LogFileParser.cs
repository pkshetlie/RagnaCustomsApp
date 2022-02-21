using RagnaCustoms.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RagnaCustoms.App.Services
{
    class LogFileParser
    {

        public event Action<Session> OnSongLoad;
        public event Action<Session> OnLevelLoad;
        public event Action<Session> OnSongEnds;
        public event Action<Session> OnScore;
        public event Action<Session> OnScoreDetail;

        private long lastLength;

        public Song Song { get; private set; }
        public Session Session { get; private set; }
        public long Position { get; private set; }

        public void ResetSession(){
            Song = new Song();
            Session = new Session();
            Session.Song = Song;
        }

        public LogFileParser()
        {
            ResetSession();

            new Thread(() =>
            {
                using var watcher = new FileSystemWatcher(Program.RagnarockSongLogsDirectoryPath, "Ragnarock.log");
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Changed += new FileSystemEventHandler(OnChange);
                watcher.Error += new ErrorEventHandler(OnError);
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;


                while (true) ;
            }).Start();
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            ReadFile();
            OnSongLoad?.Invoke(Session);

            if (e.ChangeType != WatcherChangeTypes.Changed) return;
            //new Thread(() => { _onChangeEvent(sender, e); }).Start();
        }

        private void ReadFile()
        {
            var songLevelLineHint = "LogTemp: Warning: Song level str";
            var songNameLineHint = "LogTemp: Loading song";
            var songScoreDetailLineHint = "LogTemp: Warning: Notes missed :";
            var songScoreLineHint = "raw distance =";

            using var stream = File.Open(Program.RagnarockSongLogsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            if (stream.Length < lastLength)
            {
                stream.Position = 0;
            }
            else
            {
                stream.Position = Position;
            }
            lastLength = stream.Length;

            using var reader = new StreamReader(stream);
            List<string> lines = new List<string>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (
                    line.Contains(songLevelLineHint) ||
                    line.Contains(songNameLineHint) ||
                    line.Contains(songScoreDetailLineHint) ||
                    line.Contains(songScoreLineHint)
                    )
                {
                    lines.Add(line);
                }
            }

            foreach (var line in lines)
            {
                if (line.Contains(songLevelLineHint))
                {
                    ResetSession();
                    Song.Level = line.Substring(line.IndexOf(songLevelLineHint) + songLevelLineHint.Length).Trim(' ', '.');
                    OnLevelLoad?.Invoke(Session);
                }
                else if (line.Contains(songScoreDetailLineHint))
                {
                    var startIndex = line.IndexOf(songScoreDetailLineHint) + songScoreDetailLineHint.Length;
                    var endIndex = line.IndexOf(" / Notes hit  : ");
                    Session.NotesMissed =
                        int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

                    startIndex = line.IndexOf(" / Notes hit  : ") + " / Notes hit  : ".Length;
                    endIndex = line.IndexOf(" / Notes not processed : ");
                    Session.NotesHit = int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

                    startIndex = line.IndexOf(" / Notes not processed : ") + " / Notes not processed : ".Length;
                    endIndex = line.IndexOf(" / hit accuracy : ");
                    Session.NotesNotProcessed =
                        int.Parse(line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.'));

                    startIndex = line.IndexOf(" / hit accuracy : ") + " / hit accuracy : ".Length;
                    endIndex = line.IndexOf(" / percentage : ");
                    Session.HitAccuracy = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

                    startIndex = line.IndexOf(" / hit accuracy : ") + " / hit accuracy : ".Length;
                    endIndex = startIndex + 9;
                    Session.HitAccuracy = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

                    startIndex = endIndex + 14;
                    endIndex = startIndex + 9;
                    Session.Percentage = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

                    startIndex = endIndex + 14;
                    endIndex = startIndex + 12;
                    Session.HitSpeed = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

                    startIndex = endIndex + 15;
                    endIndex = startIndex + 8;
                    Session.Percentage2 = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');

                    startIndex = endIndex + 12;
                    endIndex = line.Length;
                    var content = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');
                    Session.Combos = int.Parse(content);

                    OnScoreDetail?.Invoke(Session);
                }
                else if (line.Contains(songNameLineHint))
                {
                    var songOggPath = line.Substring(line.IndexOf(songNameLineHint) + songNameLineHint.Length)
                        .Trim(' ', '.');
                    var songDirectoryPath = Path.GetDirectoryName(songOggPath);

                    //// if log file is coming from quest, we need to download song before calculating hash file
                    //if (device != null)
                    //{
                    //    var songName = songDirectoryPath.Split('\\').Last();
                    //    var tempDir = Path.GetDirectoryName(SongLogFilePath);
                    //    var tempSongFolder = tempDir + "\\" + songName;
                    //    var oculusSongFolder = questSongDirectoryPath + "\\" + songName;
                    //    if (!device.DirectoryExists(oculusSongFolder)) continue;
                    //    device.DownloadFolder(oculusSongFolder, tempSongFolder);
                    //    songDirectoryPath = tempSongFolder;
                    //}

                    var songDirectory = new DirectoryInfo(songDirectoryPath);
                    if (!songDirectory.Exists) continue;

                    var songDatFiles = songDirectory.EnumerateFiles("*.dat");

                    var filesHashs = songDatFiles.Select(ComputeMd5)
                        .OrderBy(hash => hash);
                    var concatenatedHashs = string.Concat(filesHashs);

                    Song.Hash = ComputeMd5(concatenatedHashs);
                    Song.Folder = songOggPath;
                    OnSongLoad?.Invoke(Session);

                }
                else if (line.Contains(songScoreLineHint))
                {
                    var startIndex = line.IndexOf(songScoreLineHint) + songScoreLineHint.Length;
                    var endIndex = line.IndexOf("and adjusted distance =");
                    if (Session.Song.Hash is null) continue;
                    if (String.IsNullOrEmpty(Session.Score))
                    {
                        Session.Score = line.Substring(startIndex, endIndex - startIndex).Trim(' ', '.');
                        OnScore?.Invoke(Session);
                        OnSongEnds?.Invoke(Session);
                        Thread.Sleep(1000);
                    }
                }               
            }
            Position = stream.Position;
        }

        internal void FirstInit()
        {
            ReadFile();
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
        private static void OnError(object sender, ErrorEventArgs e) => PrintException(e.GetException());
        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}

