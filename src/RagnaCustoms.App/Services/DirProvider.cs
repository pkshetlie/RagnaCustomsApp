using RagnaCustoms.Services;
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


        public static DirectoryInfo getGameIniFile()
        {
            string str = Path.Combine(
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
           RagnarockDirectoryName,
           "Saved", "Config", "WindowsNoEditor", "Game.ini").ToString();
            return new DirectoryInfo(str);
        }

        public static DirectoryInfo getCustomDirectory()
        {
            var conf = new Configuration();
            if (String.IsNullOrEmpty(conf.BaseFolder))
            {
                conf.BaseFolder = getDefaultDirectory();
            }
            return new DirectoryInfo(conf.BaseFolder);
        }


        public IEnumerable<FileInfo> GetLocalFiles()
        {
            return getCustomDirectory().EnumerateFiles(SongSearchPattern);
        }

        public IEnumerable<Song> GetLocalSongs()
        {
            return GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);
        }
        public static string getDefaultDirectory()
        {
            string str = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        ).ToString();
            return str;
        }
    }
}