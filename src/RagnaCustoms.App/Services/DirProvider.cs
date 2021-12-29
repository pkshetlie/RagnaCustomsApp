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
        const string SongSearchPattern = "*.zip";

        public static string RagnarockSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        );

        protected virtual DirectoryInfo RagnarockSongDirectory => Directory.CreateDirectory(RagnarockSongDirectoryPath);


        public IEnumerable<FileInfo> GetLocalFiles() => RagnarockSongDirectory.EnumerateFiles(SongSearchPattern);
        public IEnumerable<Song> GetLocalSongs() => GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);

    }
}
