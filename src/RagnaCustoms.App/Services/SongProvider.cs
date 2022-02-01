using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RagnaCustoms.App.Extensions;
using RagnaCustoms.Services;

namespace RagnaCustoms.Models
{
    public class SongProvider : ISongProvider
    {
        public IEnumerable<Song> SearchLocal()
        {
            return new DirProvider().GetLocalSongs().ToList();
        }

        public IEnumerable<Song> SearchLocal(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return SearchLocal();

            return new DirProvider().GetLocalSongs().Where(song => song.Name.Contains(term)).ToList();
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
            foreach (var songpath in Directory.GetDirectories(DirProvider.RagnarockSongDirectoryPath))
            {
                var idFile = Path.Combine(songpath, ".id");
                var hashFile = Path.Combine(songpath, ".hash");
                try
                {
                    if (File.Exists(idFile))
                    {
                        var songInfo = await SearchOnlineAsync(int.Parse(File.ReadAllText(idFile)));
                        if (songInfo.Hash == File.ReadAllText(hashFile)) songInfo.UpToDate = true;
                        songs.Add(songInfo);
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return songs;
        }

        public virtual async Task DownloadAsync(int songId, Action<int> downloadProgressChanged,
            Action<bool> downloadCompleted, Action<string> downloadTitle, bool autoClose = false)
        {
            using var client = new WebClient();

            var uri = new Uri($"https://ragnacustoms.com/songs/download/{songId}");

            var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempDirectory = Directory.CreateDirectory(tempDirectoryPath);
            var tempFilePath = Path.GetTempFileName();

            var songInfo = await SearchOnlineAsync(songId);
            if (songInfo == null)
            {
                downloadCompleted?.Invoke(autoClose);
                return;
            }

            downloadTitle?.Invoke($"{songInfo.Name} by {songInfo.Mapper}");
            var songDirectoryPath = Path.Combine(DirProvider.RagnarockSongDirectoryPath,
                $"{songInfo.Name.Slug()}{songInfo.Mapper.Slug()}");

            var songBackupDirectoryPath = Path.Combine(DirProvider.RagnarockBackupSongDirectoryPath,
                $"{songInfo.Name.Slug()}{songInfo.Mapper.Slug()}");

            if (Directory.Exists(Path.Combine(songBackupDirectoryPath)))
            {
                // le dossier existe dans backup
                if (File.Exists(Path.Combine(songBackupDirectoryPath, ".hash")) &&
                    File.ReadAllText(Path.Combine(songBackupDirectoryPath, ".hash")) == songInfo.Hash)
                {
                    //le hash est OK
                    Oculus.PushSong(songDirectoryPath);
                    if (!Directory.Exists(Path.Combine(songDirectoryPath)))
                        new DirectoryInfo(songBackupDirectoryPath).MoveTo(songDirectoryPath);
                    downloadCompleted?.Invoke(autoClose);
                    return;
                }

                //le hash est périmé, on supprime et on laisse le téléchargement se faire.
                Directory.Delete(Path.Combine(songBackupDirectoryPath));
            }

            if (File.Exists(Path.Combine(songDirectoryPath, ".hash")) &&
                File.ReadAllText(Path.Combine(songDirectoryPath, ".hash")) == songInfo.Hash)
            {
                Oculus.PushSong(songDirectoryPath);
                downloadCompleted?.Invoke(autoClose);
                return;
            }


            client.DownloadProgressChanged +=
                (sender, args) => downloadProgressChanged?.Invoke(args.ProgressPercentage);
            client.DownloadFileCompleted += ClientDownloadFileCompleted;
            client.DownloadFileAsync(uri, tempFilePath);

            void ClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                ZipFile.ExtractToDirectory(tempFilePath, tempDirectoryPath);

                var songDirectory = tempDirectory.EnumerateDirectories().First();
                var ragnarockSongDirectoryPath = Path.Combine(DirProvider.RagnarockSongDirectoryPath,
                    $"{songDirectory.Name}{songInfo.Mapper.Slug()}");

                if (Directory.Exists(ragnarockSongDirectoryPath))
                    Directory.Delete(ragnarockSongDirectoryPath, true);

                if (Path.GetPathRoot(ragnarockSongDirectoryPath) == songDirectory.Root.FullName)
                {
                    songDirectory.MoveTo(ragnarockSongDirectoryPath);
                }
                else
                {
                    Directory.CreateDirectory(ragnarockSongDirectoryPath);

                    var files = songDirectory.GetFiles();
                    foreach (var file in files)
                    {
                        var tempPath = Path.Combine(ragnarockSongDirectoryPath, file.Name);
                        file.CopyTo(tempPath, false);
                    }
                }

                using (var writer = new StreamWriter(Path.Combine(ragnarockSongDirectoryPath, ".hash"), false))
                {
                    writer.Write(songInfo.Hash);
                }

                using (var writer = new StreamWriter(Path.Combine(ragnarockSongDirectoryPath, ".id"), false))
                {
                    writer.Write(songInfo.Id);
                }

                File.Delete(tempFilePath);
                Directory.Delete(tempDirectoryPath, true);

                Oculus.PushSong(ragnarockSongDirectoryPath);

                downloadCompleted?.Invoke(autoClose);
            }
        }
    }
}