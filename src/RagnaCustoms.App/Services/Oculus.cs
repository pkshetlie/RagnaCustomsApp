using RagnaCustoms.App.Views;
using System;
using System.Linq;
using System.IO;
using MediaDevices;
using System.Windows.Forms;

namespace RagnaCustoms.Services
{
    class Oculus
    {

        const string QuestSongDirectoryName = @"\Android\data\com.wanadev.ragnarockquest\files\UE4Game\Ragnarock\Ragnarock\Saved\CustomSongs";

        const string RagnarockDirectoryName = "Ragnarock";
        const string SongDirectoryName = "CustomSongs";
        static readonly string LocalRagnarockSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        );


        public static MediaDevice GetDevice()
        { 
            var devices = MediaDevice.GetDevices();
            foreach (var d in devices)
            {
                if (d.Description == "Quest 2")
                {
                    return d;
                }
                else if (d.Description == "Quest")
                {
                    return d;
                }
            }
            return null;
        }

        public static int SyncSongs()
        {
           
            var syncingView = new OculusSyncForm();
            var device = GetDevice();
            if (device != null)
            {
                try
                {
                    device.Connect();
                    var base_folder = device.GetDirectories(@"\")[0];
                    var QuestSongDirectoryPath = $"{base_folder}{QuestSongDirectoryName}";

                    var songs = Directory.GetDirectories(LocalRagnarockSongDirectoryPath);
                    syncingView.SyncingProgressBar.Minimum = 0;
                    syncingView.SyncingProgressBar.Maximum = songs.Length;
                    syncingView.SyncingProgressBar.Step = 1;
                    syncingView.Show();

                    if (!device.IsConnected)
                    {
                        throw new NotConnectedException("Not connected");
                    }

                    syncingView.SyncingLabel.Text = "Creating CustomSongs directory";
                    if (!device.DirectoryExists(QuestSongDirectoryPath))
                    {
                        device.CreateDirectory(QuestSongDirectoryPath);
                    }

                    foreach (var song in songs)
                    {
                        var folderName = song.Split(Path.DirectorySeparatorChar).Last();
                        var songToSync = $"{LocalRagnarockSongDirectoryPath}\\{folderName}";
                        var destinationFolder = $"{QuestSongDirectoryPath}\\{folderName}";

                        /*syncingView.SyncingLabel.Text = $"Copying {folderName}";
                        if (device.DirectoryExists(destinationFolder))
                        {
                            device.DeleteDirectory(destinationFolder, true);
                        }*/
                        /*device.CreateDirectory(destinationFolder);*/

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
                    MessageBox.Show($"Error: {except.Message}", "RagnaCutoms", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 3;
                }
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
                var base_folder = device.GetDirectories(@"\")[0];
                var QuestSongDirectoryPath = $"{base_folder}{QuestSongDirectoryName}\\{song}";

                if (!device.IsConnected)
                {
                    throw new NotConnectedException("Not connected");
                }

                if (device.DirectoryExists(QuestSongDirectoryPath))
                {
                    device.DeleteDirectory(QuestSongDirectoryPath, true);
                }

                device.CreateDirectory(QuestSongDirectoryPath);

                device.UploadFolder(path, QuestSongDirectoryPath);
                device.Disconnect();

                return 0;
            }

            return 1;
        }
    }
}
