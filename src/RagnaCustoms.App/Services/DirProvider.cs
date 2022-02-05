using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RagnaCustoms.Models
{
    public class DirProvider
    {
        private const string RagnarockDirectoryName = "Ragnarock";
        private const string SongDirectoryName = "CustomSongs";
        private const string BackupSongDirectoryName = "bkp-" + SongDirectoryName;
        private const string SongSearchPattern = "*.zip";

        public static readonly string RagnarockSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        );

        public static readonly string RagnarockBackupSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            BackupSongDirectoryName
        );


        public DirectoryInfo RagnarockSongDirectory => Directory.CreateDirectory(RagnarockSongDirectoryPath);
        public DirectoryInfo RagnarockSongBkpDirectory => Directory.CreateDirectory(RagnarockBackupSongDirectoryPath);

        public static DirectoryInfo getCustomDirectory()
        {
            return new DirectoryInfo(RagnarockSongDirectoryPath);
        }

        public static DirectoryInfo getCustomBackupDirectory()
        {
            return new DirectoryInfo(RagnarockBackupSongDirectoryPath);
        }


        public IEnumerable<FileInfo> GetLocalFiles()
        {
            return RagnarockSongDirectory.EnumerateFiles(SongSearchPattern);
        }

        public IEnumerable<Song> GetLocalSongs()
        {
            return GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);
        }
    }
}