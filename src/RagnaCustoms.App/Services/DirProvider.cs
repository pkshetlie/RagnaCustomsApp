using RagnaCustoms.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RagnaCustoms.App.Extensions;

namespace RagnaCustoms.Models
{
    public class DirProvider
    {
        const string RagnarockDirectoryName = "Ragnarock";
        const string SongDirectoryName = "CustomSongs";
        const string BackupSongDirectoryName = "bkp-" + SongDirectoryName;
        const string SongSearchPattern = "*.zip";

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

        public static DirectoryInfo getCustomDirectory()
        {
            return new DirectoryInfo(RagnarockSongDirectoryPath);
        }
        public static DirectoryInfo getCustomBackupDirectory()
        {
            return new DirectoryInfo(RagnarockBackupSongDirectoryPath);
        }
        
        
        public DirectoryInfo RagnarockSongDirectory => Directory.CreateDirectory(RagnarockSongDirectoryPath);
        public DirectoryInfo RagnarockSongBkpDirectory => Directory.CreateDirectory(RagnarockBackupSongDirectoryPath);


        public IEnumerable<FileInfo> GetLocalFiles() => RagnarockSongDirectory.EnumerateFiles(SongSearchPattern);
        public IEnumerable<Song> GetLocalSongs() => GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);

    }
}
