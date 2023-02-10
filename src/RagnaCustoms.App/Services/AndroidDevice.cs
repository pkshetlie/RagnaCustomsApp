using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MediaDevices;
using RagnaCustoms.App.Views;

namespace RagnaCustoms.Services
{
    internal class AndroidDevice
    {
        public static string deviceSufix = "quest";

        public static string DeviceSongDirectoryName
        {
            get; set;
        }
        public static string DeviceConfigDirectoryName
        {
            get; set;
        }

        private const string RagnarockDirectoryName = "Ragnarock";
        private const string SongDirectoryName = "CustomSongs";

        private static string LocalRagnarockSongDirectoryPath
        {
            get { return new Configuration().BaseFolder; }
        }


        public static MediaDevice GetFirstFoundDevice()
        {
            var devices = MediaDevice.GetDevices();
            foreach (var d in devices)
                if (d.Description.ToLower().Contains("quest") || d.Description.ToLower().Contains("pico"))
                {
                    DeviceSongDirectoryName = $@"\Android\data\com.wanadev.ragnarockquest\files\UE4Game\Ragnarock\Ragnarock\Saved\CustomSongs";
                    DeviceConfigDirectoryName = $@"\Android\data\com.wanadev.ragnarockquest\files\UE4Game\Ragnarock\Ragnarock\Saved\Config";
                    return d;
                }
            return null;
        }

        public static int SyncSongs()
        {
            var syncingView = new AndroidDeviceSyncForm();
            var device = GetFirstFoundDevice();
            if (device != null)
                try
                {
                    device.Connect();
                    var baseFolder = device.GetDirectories(@"\")[0];
                    var questSongDirectoryPath = $"{baseFolder}{DeviceSongDirectoryName}";

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
            var device = GetFirstFoundDevice();
            if (device != null)
            {
                var song = path.Split(Path.DirectorySeparatorChar).Last();
                device.Connect();
                var baseFolder = device.GetDirectories(@"\")[0];
                var questSongDirectoryPath = $"{baseFolder}{DeviceSongDirectoryName}\\{song}";

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

        internal static void GenerateGameIni()
        {
            var device = AndroidDevice.GetFirstFoundDevice();
            var _configuration = new Configuration();
            if (device != null)
            {
                device.Connect();
                var localgameIni = $"{_configuration.BaseFolder}\\Ragnarock.ini";
                using (StreamWriter writer = new StreamWriter(localgameIni))
                {
                    writer.WriteLine($"{{\"CustomApiURLs\":[\"https://api.ragnacustoms.com/wanapi/score/{_configuration.ApiKey}\"]}}");
                }

                var baseFolder = device.GetDirectories(@"\")[0];

                var gameIni = $"{baseFolder}{DeviceConfigDirectoryName}\\Ragnarock.ini";
                if (device.FileExists(gameIni))
                {
                    device.DeleteFile(gameIni);
                }
                if (!device.DirectoryExists($"{baseFolder}{DeviceConfigDirectoryName}"))
                {
                    device.CreateDirectory($"{baseFolder}{DeviceConfigDirectoryName}");
                }
                device.UploadFile(localgameIni, gameIni);

                File.Delete(localgameIni);

                device.Disconnect();
            }


        }
    }
}