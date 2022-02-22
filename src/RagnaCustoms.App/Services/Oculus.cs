using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MediaDevices;
using RagnaCustoms.App;
using RagnaCustoms.App.Views;

namespace RagnaCustoms.Services
{
    internal class Oculus
    {
        public const string QuestSongDirectoryName =
            @"\Android\data\com.wanadev.ragnarockquest\files\UE4Game\Ragnarock\Ragnarock\Saved\CustomSongs";

        private const string QuestLogDirectoryName =
            @"\Android\data\com.wanadev.ragnarockquest\files\UE4Game\Ragnarock\Ragnarock\Saved\Logs";

        private const string RagnarockDirectoryName = "Ragnarock";
        private const string SongDirectoryName = "CustomSongs";

        private static readonly string LocalRagnarockSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        );


        public static MediaDevice GetDevice()
        {
            var devices = MediaDevice.GetDevices();
            foreach (var d in devices)
                if (d.Description == "Quest 2")
                    return d;
                else if (d.Description == "Quest") return d;
            return null;
        }

        public static int SyncSongs()
        {
            var syncingView = new OculusSyncForm();
            var device = GetDevice();
            if (device != null)
                try
                {
                    device.Connect();
                    var baseFolder = device.GetDirectories(@"\")[0];
                    var questSongDirectoryPath = $"{baseFolder}{QuestSongDirectoryName}";

                    var songs = Directory.GetDirectories(LocalRagnarockSongDirectoryPath);
                    syncingView.SyncingProgressBar.Minimum = 0;
                    syncingView.SyncingProgressBar.Maximum = songs.Length;
                    syncingView.SyncingProgressBar.Step = 1;
                    syncingView.Show();

                    if (!device.IsConnected) throw new NotConnectedException("Not connected");

                    syncingView.SyncingLabel.Text = "Creating CustomSongs directory";
                    if (device.DirectoryExists(questSongDirectoryPath))
                        device.DeleteDirectory(questSongDirectoryPath, true);

                    device.CreateDirectory(questSongDirectoryPath);


                    foreach (var song in songs)
                    {
                        var folderName = song.Split(Path.DirectorySeparatorChar).Last();
                        var songToSync = $"{LocalRagnarockSongDirectoryPath}\\{folderName}";
                        var destinationFolder = $"{questSongDirectoryPath}\\{folderName}";

                        syncingView.SyncingLabel.Text = $"Copying {folderName}";
                        if (device.DirectoryExists(destinationFolder)) device.DeleteDirectory(destinationFolder, true);
                        device.CreateDirectory(destinationFolder);

                        device.UploadFolder(songToSync, destinationFolder);
                        syncingView.SyncingProgressBar.PerformStep();
                    }


                    device.Disconnect();
                    syncingView.Close();
                    return 0;
                }
                catch (Exception except)
                {
                    syncingView.Close();
                    MessageBox.Show($"Error: {except.Message}", "RagnaCutoms", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return 3;
                }

            return 1;
        }

        public static int PushSong(string path)
        {
            var device = GetDevice();
            if (device != null)
            {
                var song = path.Split(Path.DirectorySeparatorChar).Last();
                device.Connect();
                var baseFolder = device.GetDirectories(@"\")[0];
                var questSongDirectoryPath = $"{baseFolder}{QuestSongDirectoryName}\\{song}";

                if (!device.IsConnected) throw new NotConnectedException("Not connected");

                if (device.DirectoryExists(questSongDirectoryPath))
                    device.DeleteDirectory(questSongDirectoryPath, true);

                device.CreateDirectory(questSongDirectoryPath);

                device.UploadFolder(path, questSongDirectoryPath);
                device.Disconnect();

                return 0;
            }

            return 1;
        }

        public static int SendScore()
        {
            var device = GetDevice();
            if (device != null)
            {
                var configuration = new Configuration();
                var sessionUploader = new SessionUploader(configuration, Program.UploadSessionUri);

                device.Connect();
                var base_folder = device.GetDirectories(@"\")[0];

                var QuestLogDirectoryName = $"{base_folder}{Oculus.QuestLogDirectoryName}";
                var tempPath = Path.GetTempPath() + Guid.NewGuid();

                if (!device.IsConnected) throw new NotConnectedException("Not connected");

                if (device.DirectoryExists(QuestLogDirectoryName))
                    device.DownloadFolder(QuestLogDirectoryName, tempPath);

                var logFiles = Directory.GetFiles(tempPath);
                foreach (var logFile in logFiles)
                {
                    var songResultParser = new SessionParser(logFile);

                    songResultParser.OnNewSession += async session =>
                        await sessionUploader.UploadAsync(session);
                    songResultParser.StartAsync(device);
                }

                Directory.Delete(tempPath, true);
                return 0;
            }

            return 1;
        }
    }
}