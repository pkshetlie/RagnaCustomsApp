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
    public class SongProvider : ISongProvider
    {
        const string RagnarockDirectoryName = "Ragnarock";
        const string SongDirectoryName = "CustomSongs";
        const string SongSearchPattern = "*.zip";

        static readonly string RagnarockSongDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            RagnarockDirectoryName,
            SongDirectoryName
        );

        protected virtual DirectoryInfo RagnarockSongDirectory { get; }

        public SongProvider()
        {
            RagnarockSongDirectory = Directory.CreateDirectory(RagnarockSongDirectoryPath);
        }

        public IEnumerable<Song> SearchLocal()
        {
            return GetLocalSongs().ToList();
        }

        public IEnumerable<Song> SearchLocal(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return SearchLocal();
            }

            return GetLocalSongs().Where(song => song.Name.Contains(term)).ToList();
        }

        public async Task<SongSearchModel> SearchOnlineAsync(int id)
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://ragnacustoms.com/api/song/{id}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<SongSearchModel>(content);

                return searchResult;
            }

            return null;
        }

        public async Task<IEnumerable<SongSearchModel>> SearchOnlineAsync(string term)
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://ragnacustoms.com/api/search/{term}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<Result<SongSearchModel>>(content);

                return searchResult.Results;
            }

            return Enumerable.Empty<SongSearchModel>();
        }

        public async Task<IEnumerable<SongSearchModel>> CompareSongsWithOnlineAsync()
        {
            var songs = new List<SongSearchModel>();
            foreach (var songpath in Directory.GetDirectories(RagnarockSongDirectoryPath))
            {
                var idFile = Path.Combine(songpath, ".id");
                var hashFile = Path.Combine(songpath, ".hash");
                if (File.Exists(idFile)){
                    var songInfo = await SearchOnlineAsync(int.Parse(File.ReadAllText(idFile)));
                    if (songInfo.Hash == File.ReadAllText(hashFile))
                    {
                        songInfo.UpToDate = true;
                    }
                    songs.Add(songInfo);
                }
            }
            return songs;
        }

        public virtual async Task DownloadAsync(int songId, Action<int> downloadProgressChanged, Action<bool> downloadCompleted, bool autoClose = false)
        {
            using var client = new WebClient();

            var uri = new Uri($"https://ragnacustoms.com/songs/download/{songId}");

            var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempDirectory = Directory.CreateDirectory(tempDirectoryPath);
            var tempFilePath = Path.GetTempFileName();

            var songInfo = await SearchOnlineAsync(songId);
            var SongDirectoryPath = Path.Combine(RagnarockSongDirectoryPath, $"{songInfo.Name.Slug()}{songInfo.Mapper.Slug()}");
            if (File.Exists(Path.Combine(SongDirectoryPath, ".hash")) && File.ReadAllText(Path.Combine(SongDirectoryPath, ".hash")) == songInfo.Hash)
            {
                Oculus.PushSong(SongDirectoryPath);
                downloadCompleted?.Invoke(autoClose);
                return;
            }


            client.DownloadProgressChanged += (sender, args) => downloadProgressChanged?.Invoke(args.ProgressPercentage);
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            client.DownloadFileAsync(uri, tempFilePath);

            void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                ZipFile.ExtractToDirectory(tempFilePath, tempDirectoryPath);

                var songDirectory = tempDirectory.EnumerateDirectories().First();
                var ragnarockSongDirectoryPath = Path.Combine(RagnarockSongDirectoryPath, $"{songDirectory.Name}{songInfo.Mapper.Slug()}");

                if (Directory.Exists(ragnarockSongDirectoryPath))
                    Directory.Delete(ragnarockSongDirectoryPath, recursive: true);

                if (Path.GetPathRoot(ragnarockSongDirectoryPath) == songDirectory.Root.FullName)
                {
                    songDirectory.MoveTo(ragnarockSongDirectoryPath);
                }
                else
                {
                    Directory.CreateDirectory(ragnarockSongDirectoryPath);

                    FileInfo[] files = songDirectory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        string tempPath = Path.Combine(ragnarockSongDirectoryPath, file.Name);
                        file.CopyTo(tempPath, false);
                    }
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(ragnarockSongDirectoryPath, ".hash"), false))
                {
                    writer.Write(songInfo.Hash);
                }

                using (StreamWriter writer = new StreamWriter(Path.Combine(ragnarockSongDirectoryPath, ".id"), false))
                {
                    writer.Write(songInfo.Id);
                }

                File.Delete(tempFilePath);
                Directory.Delete(tempDirectoryPath, recursive: true);

                Oculus.PushSong(ragnarockSongDirectoryPath);

                downloadCompleted?.Invoke(autoClose);
            }
        }

        protected virtual IEnumerable<FileInfo> GetLocalFiles() => RagnarockSongDirectory.EnumerateFiles(SongSearchPattern);
        protected virtual IEnumerable<Song> GetLocalSongs() => GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);

     
    }
}
