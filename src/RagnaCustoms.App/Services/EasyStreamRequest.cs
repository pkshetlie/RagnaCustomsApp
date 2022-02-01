using System;
using System.IO;
using System.Security.Principal;
using RagnaCustoms.Services;

namespace RagnaCustoms.Models
{
    public class EasyStreamRequest
    {
        private static readonly DirectoryInfo backupDirectory = DirProvider.getCustomBackupDirectory();
        private static readonly DirectoryInfo songsDirectory = DirProvider.getCustomDirectory();

        public static void EnableEasyStreamRequest(Configuration configuration)
        {
            configuration.EasyStreamRequest = true;
        }

        public static void DisableEasyStreamRequest(Configuration configuration)
        {
            configuration.EasyStreamRequest = false;
            RestoreCustomSongDirectory();
        }

        public static void CreateBackupDirectory()
        {
            if (!songsDirectory.Exists) songsDirectory.Create();
            foreach (var subdir in songsDirectory.GetDirectories())
                subdir.MoveTo(Path.Combine(DirProvider.RagnarockBackupSongDirectoryPath, subdir.Name));
        }

        public static void RestoreCustomSongDirectory()
        {
            if (!backupDirectory.Exists) return;
            CopySongsOnBackup();
            //songsDirectory.Delete(true);
            backupDirectory.Refresh();
            var collection = Directory.GetAccessControl(backupDirectory.FullName)
                .GetAccessRules(true, true, typeof(NTAccount));
            foreach (var subdir in backupDirectory.GetDirectories())
                subdir.MoveTo(Path.Combine(DirProvider.RagnarockSongDirectoryPath, subdir.Name));
        }

        public static void CopySongsOnBackup()
        {
            if (!songsDirectory.Exists) songsDirectory.Create();
            foreach (var fileInfo in DirProvider.getCustomDirectory().GetDirectories())
            {
                var tempDir = new DirectoryInfo(Path.Combine(backupDirectory.FullName, fileInfo.Name));
                if (tempDir.Exists) tempDir.Delete(true);
                fileInfo.MoveTo(tempDir.FullName);
            }
        }

        public static void MoveSongOnBackup(DirectoryInfo songDirectory)
        {
            foreach (var fileInfo in DirProvider.getCustomDirectory().GetDirectories())
                if (fileInfo.Name == songDirectory.Name)
                {
                    var tempDir = new DirectoryInfo(Path.Combine(backupDirectory.FullName, fileInfo.Name));
                    if (tempDir.Exists) tempDir.Delete(true);
                    try
                    {
                        fileInfo.MoveTo(tempDir.FullName);
                    }
                    catch (Exception e)
                    {
                        // TODO : sa marche, il déplace bien... mais sa va quand même passé ici jsp pk
                        /* Ignored */
                    }
                }
        }
    }
}