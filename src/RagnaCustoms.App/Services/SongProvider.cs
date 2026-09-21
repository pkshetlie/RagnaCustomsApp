using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RagnaCustoms.App.Extensions;
using RagnaCustoms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RagnaCustoms.Models
{
    public class SongProvider : ISongProvider
    {
        private Configuration configuration;

        public SongProvider()
        {
            configuration = new Configuration();
        }
        public IEnumerable<Song> SearchLocal()
        {
            return new DirProvider().GetLocalSongs().ToList();
        }

        public IEnumerable<Song> SearchLocal(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return SearchLocal();

            return new DirProvider().GetLocalSongs().Where(song => song.Name.Contains(term)).ToList();
        }

        public async Task<SongSearchModel> SearchOneOnlineAsync(string id)
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://api.ragnacustoms.com/api/song/{id}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<SongSearchModel>(content);

                return searchResult;
            }

            return null;
        }

        public async Task<List<SongSearchModel>> CheckUpdateAsync()
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://api.ragnacustoms.com/api/song/check-updates");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<List<SongSearchModel>>(content);

                return searchResult;
            }

            return new List<SongSearchModel>();
        }

        public async Task<IEnumerable<SongSearchModel>> SearchOnlineAsync(string term)
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://api.ragnacustoms.com/api/search/{term}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonConvert.DeserializeObject<Result<SongSearchModel>>(content);

                var songs = searchResult?.Results?.ToList() ?? new List<SongSearchModel>();
                MarkInstalledSongs(songs, DirProvider.getCustomDirectory().FullName);
                return songs;
            }

            return Enumerable.Empty<SongSearchModel>();
        }

        public IEnumerable<SongSearchModel> exploreRecurs(string dir, List<SongSearchModel> songsInfo)
        {

            var songs = new BindingList<SongSearchModel>();
            foreach (var songpath in Directory.GetDirectories(dir))
            {
                var idFile = Path.Combine(songpath, ".id");
                if (File.Exists(idFile))
                {
                    var hashFile = Path.Combine(songpath, ".hash");
                    var localId = File.ReadAllText(idFile).Trim();
                    var songInfo = songsInfo.FirstOrDefault(x =>
                        string.Equals(x.Id?.Trim(), localId, StringComparison.OrdinalIgnoreCase));
                    if (songInfo != null)
                    {
                        songInfo.CurrentFolder = songpath;
                        songInfo.IsInstalled = true;
                        if (File.Exists(hashFile) && string.Equals(
                                songInfo.Hash?.Trim(),
                                File.ReadAllText(hashFile).Trim(),
                                StringComparison.OrdinalIgnoreCase))
                        {
                            songInfo.UpToDate = true;
                        }
                        songs.Add(songInfo);
                    }
                }
                else
                {
                    foreach (var song in exploreRecurs(Path.Combine(dir, songpath), songsInfo))
                    {
                        songs.Add(song);
                    }
                }
            }
            return songs;

        }

        private static void MarkInstalledSongs(List<SongSearchModel> songs, string rootDirectory)
        {
            if (songs == null || songs.Count == 0 || !Directory.Exists(rootDirectory)) return;

            foreach (var directory in EnumerateDirectoriesSafe(rootDirectory))
            {
                var idFile = Path.Combine(directory, ".id");
                if (!File.Exists(idFile)) continue;

                var localId = File.ReadAllText(idFile).Trim();
                var song = songs.FirstOrDefault(item =>
                    string.Equals(item.Id?.Trim(), localId, StringComparison.OrdinalIgnoreCase));
                if (song == null) continue;

                song.IsInstalled = true;
                song.CurrentFolder = directory;

                var hashFile = Path.Combine(directory, ".hash");
                if (File.Exists(hashFile))
                {
                    var localHash = File.ReadAllText(hashFile).Trim();
                    song.UpToDate = string.Equals(song.Hash?.Trim(), localHash, StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private static IEnumerable<string> EnumerateDirectoriesSafe(string rootDirectory)
        {
            IEnumerable<string> children;
            try
            {
                children = Directory.EnumerateDirectories(rootDirectory).ToList();
            }
            catch (IOException)
            {
                yield break;
            }
            catch (UnauthorizedAccessException)
            {
                yield break;
            }

            foreach (var child in children)
            {
                yield return child;

                foreach (var nested in EnumerateDirectoriesSafe(child))
                {
                    yield return nested;
                }
            }
        }


        public async Task<IEnumerable<SongSearchModel>> CompareSongsWithOnlineAsync(string dir = null)
        {
            var songsInfo = await CheckUpdateAsync();

            if (dir == null)
            {
                dir = DirProvider.getCustomDirectory().ToString();
            }

            return exploreRecurs(dir, songsInfo);
        }

        public virtual async Task DownloadAsync(string songId, Action<int> downloadProgressChanged,
            Action<bool> downloadCompleted, Action<string> downloadTitle, bool autoClose = false, string songFolder = null, string subfolder = null)
        {
            using var client = new WebClient();
            var configuration = new Configuration();
            var uri = new Uri($"https://api.ragnacustoms.com/songs/download/{songId}");

            if (!string.IsNullOrWhiteSpace(configuration.ApiKey))
            {
                uri = new Uri($"https://api.ragnacustoms.com/songs/download/{songId}/{configuration.ApiKey}");
            }

            var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempDirectory = Directory.CreateDirectory(tempDirectoryPath);
            var tempFilePath = Path.GetTempFileName();

            var songInfo = await SearchOneOnlineAsync(songId);
            if (songInfo == null)
            {
                downloadCompleted?.Invoke(autoClose);
                return;
            }

            var songDir = SongFolderNameFormatter.Format(songInfo, configuration.SongFolderPattern, DateTime.Now);

            downloadTitle?.Invoke($"{songInfo.Name} by {songInfo.Mapper}");
            var songDirectoryPath = BuildSongDirectoryPath(songInfo, configuration, songDir);

            var rankedDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Ranked", songDir);

            if (configuration.CopyRanked)
            {
                var RankedDir = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Ranked");
                if (!Directory.Exists(RankedDir))
                {
                    Directory.CreateDirectory(RankedDir);
                }
            }

            if (songFolder != null)
            {
                songDirectoryPath = songFolder;
            }

            if (subfolder != null)
            {
                songDirectoryPath = Path.Combine(configuration.BaseFolder, subfolder);//, songDir);
                if (!Directory.Exists(songDirectoryPath))
                {
                    Directory.CreateDirectory(songDirectoryPath);
                }
                songDirectoryPath = Path.Combine(songDirectoryPath, songDir);
            }
            //if (File.Exists(Path.Combine(songDirectoryPath, ".hash")) &&
            //    File.ReadAllText(Path.Combine(songDirectoryPath, ".hash")) == songInfo.Hash && (configuration.CopyRanked && ))
            //{

            //    downloadCompleted?.Invoke(autoClose);
            //    return;
            //}


            client.DownloadProgressChanged += (sender, args) => downloadProgressChanged?.Invoke(args.ProgressPercentage);
            client.DownloadFileCompleted += ClientDownloadFileCompleted;
            client.DownloadFileAsync(uri, tempFilePath);

            void ClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
            {
                ZipFile.ExtractToDirectory(tempFilePath, tempDirectoryPath);

                var songDirectory = tempDirectory.EnumerateDirectories().First();

                if (Directory.Exists(songDirectoryPath))
                    Directory.Delete(songDirectoryPath, true);

                if (Path.GetPathRoot(songDirectoryPath) == songDirectory.Root.FullName)
                {
                    songDirectory.MoveTo(songDirectoryPath);
                }
                else
                {
                    Directory.CreateDirectory(songDirectoryPath);

                    var files = songDirectory.GetFiles();
                    foreach (var file in files)
                    {
                        var tempPath = Path.Combine(songDirectoryPath, file.Name);
                        file.CopyTo(tempPath, false);
                    }
                }
                if (configuration.CopyRanked && songInfo.IsRanked)
                {
                    if (Directory.Exists(rankedDirectoryPath))
                    {
                        Directory.Delete(rankedDirectoryPath, true);
                    }
                    Directory.CreateDirectory(rankedDirectoryPath);

                    var files = songDirectory.GetFiles();
                    foreach (var file in files)
                    {
                        var tempPath = Path.Combine(rankedDirectoryPath, file.Name);
                        file.CopyTo(tempPath, false);
                    }
                }

                using (var writer = new StreamWriter(Path.Combine(songDirectoryPath, ".hash"), false))
                {
                    writer.Write(songInfo.Hash);
                }

                using (var writer = new StreamWriter(Path.Combine(songDirectoryPath, ".id"), false))
                {
                    writer.Write(songInfo.Id);
                }

                File.Delete(tempFilePath);
                Directory.Delete(tempDirectoryPath, true);

                AndroidDevice.PushSong(songDirectoryPath);

                downloadCompleted?.Invoke(autoClose);
            }
        }

        private static string BuildSongDirectoryPath(SongSearchModel songInfo, Configuration configuration, string songFolderName)
        {
            var customDirectory = DirProvider.getCustomDirectory().ToString();
            var songDirectoryPath = Path.Combine(customDirectory, songFolderName);
            var forceSingleFolder = songFolderName.Length > 0 && char.IsDigit(songFolderName[0]);

            if (!forceSingleFolder && configuration.OrderAlphabetically)
            {
                songDirectoryPath = Path.Combine(customDirectory, "Alphabet");
                Directory.CreateDirectory(songDirectoryPath);

                var firstLetter = songInfo.Name.Slug().Substring(0, 1).ToLowerInvariant();
                songDirectoryPath = Path.Combine(songDirectoryPath, firstLetter);
                Directory.CreateDirectory(songDirectoryPath);
                songDirectoryPath = Path.Combine(songDirectoryPath, songFolderName);
            }

            if (!forceSingleFolder && configuration.OrderMapper)
            {
                songDirectoryPath = Path.Combine(customDirectory, "Mapper");
                Directory.CreateDirectory(songDirectoryPath);

                songDirectoryPath = Path.Combine(songDirectoryPath, songInfo.Mapper.Slug());
                Directory.CreateDirectory(songDirectoryPath);
                songDirectoryPath = Path.Combine(songDirectoryPath, songFolderName);
            }

            return songDirectoryPath;
        }


        public virtual async Task DownloadListAsync(int listId, Action<int> downloadProgressChanged,
           Action<bool> downloadCompleted, Action<string> downloadTitle, bool autoClose = false)
        {

            using var webClient = new WebClient();
            var json = webClient.DownloadString("https://api.ragnacustoms.com/api/song-list/" + listId);
            var stuff = JsonConvert.DeserializeObject<List<SongSearchModel>>(json);
            var i = 0;

            foreach (var songInfo in stuff)
            {
                var songId = songInfo.Id;
                using var client = new WebClient();
                var uri = new Uri($"https://api.ragnacustoms.com/songs/download/{songId}");

                if (!string.IsNullOrWhiteSpace(configuration.ApiKey))
                {
                    uri = new Uri($"https://api.ragnacustoms.com/songs/download/{songId}/{configuration.ApiKey}");
                }

                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                var tempDirectory = Directory.CreateDirectory(tempDirectoryPath);
                var tempFilePath = Path.GetTempFileName();


                var songDir = SongFolderNameFormatter.Format(songInfo, configuration.SongFolderPattern, DateTime.Now);
                var songDirectoryPath = BuildSongDirectoryPath(songInfo, configuration, songDir);

                var rankedDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Ranked", songDir);

                //if (File.Exists(Path.Combine(songDirectoryPath, ".hash")) &&
                //    File.ReadAllText(Path.Combine(songDirectoryPath, ".hash")) == songInfo.Hash)
                //{
                //    Oculus.PushSong(songDirectoryPath);
                //    i = i + 1;
                //    var percentage = (int)Math.Round((double)i / (double)stuff.Count() * 100);

                //    downloadProgressChanged?.Invoke(percentage);
                //    if (i >= stuff.Count())
                //    {
                //        downloadTitle?.Invoke($"Finish");
                //    }
                //    else
                //    {
                //        downloadTitle?.Invoke($"{percentage}% {songInfo.Name} by {songInfo.Mapper}");
                //    }
                //    continue;
                //}

                client.DownloadFileCompleted += ClientDownloadFileCompleted;
                client.DownloadFileAsync(uri, tempFilePath);

                void ClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
                {
                    i = i + 1;
                    var percentage = (int)Math.Round((double)i / (double)stuff.Count() * 100);

                    downloadTitle?.Invoke($"{percentage}% {songInfo.Name} by {songInfo.Mapper}");

                    ZipFile.ExtractToDirectory(tempFilePath, tempDirectoryPath);

                    var songDirectory = tempDirectory.EnumerateDirectories().First();


                    if (Directory.Exists(songDirectoryPath))
                        Directory.Delete(songDirectoryPath, true);

                    if (Path.GetPathRoot(songDirectoryPath) == songDirectory.Root.FullName)
                    {
                        songDirectory.MoveTo(songDirectoryPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(songDirectoryPath);

                        var files = songDirectory.GetFiles();
                        foreach (var file in files)
                        {
                            var tempPath = Path.Combine(songDirectoryPath, file.Name);
                            file.CopyTo(tempPath, false);
                        }
                    }

                    if (configuration.CopyRanked && songInfo.IsRanked)
                    {
                        if (Directory.Exists(rankedDirectoryPath))
                        {
                            Directory.Delete(rankedDirectoryPath, true);
                        }
                        Directory.CreateDirectory(rankedDirectoryPath);

                        var files = songDirectory.GetFiles();
                        foreach (var file in files)
                        {
                            var tempPath = Path.Combine(rankedDirectoryPath, file.Name);
                            file.CopyTo(tempPath, false);
                        }
                    }

                    downloadProgressChanged?.Invoke(percentage);
                    if (i >= stuff.Count())
                    {
                        downloadTitle?.Invoke($"Finish");
                    }
                    else
                    {
                        downloadTitle?.Invoke($"{percentage}% {songInfo.Name} by {songInfo.Mapper}");
                    }

                    using (var writer = new StreamWriter(Path.Combine(songDirectoryPath, ".hash"), false))
                    {
                        writer.Write(songInfo.Hash);
                    }

                    using (var writer = new StreamWriter(Path.Combine(songDirectoryPath, ".id"), false))
                    {
                        writer.Write(songInfo.Id);
                    }

                    File.Delete(tempFilePath);
                    Directory.Delete(tempDirectoryPath, true);

                    AndroidDevice.PushSong(songDirectoryPath);
                    if (i >= stuff.Count())
                    {
                        downloadCompleted?.Invoke(autoClose);
                    }
                }

            }
        }
        public virtual async Task DownloadPlaylistAsync(int playlistId, Action<int> downloadProgressChanged,
            Action<bool> downloadCompleted, Action<string> downloadTitle, Action<string> downloadError,
            bool autoClose = false)
        {
            if (string.IsNullOrWhiteSpace(configuration.ApiKey))
            {
                await Task.Yield();
                downloadError?.Invoke("A valid API key is required to download a playlist.");
                return;
            }

            List<SongSearchModel> songs;
            try
            {
                using var webClient = new WebClient();
                webClient.Headers["X-API-Key"] = configuration.ApiKey;
                var json = await webClient.DownloadStringTaskAsync(
                    new Uri("https://api.ragnacustoms.com/api/playlist/" + playlistId));
                var payload = JToken.Parse(json);
                songs = payload.Type == JTokenType.Array
                    ? payload.ToObject<List<SongSearchModel>>()
                    : payload["songs"]?.ToObject<List<SongSearchModel>>();
            }
            catch (WebException exception)
            {
                var status = (exception.Response as HttpWebResponse)?.StatusCode;
                var message = status == HttpStatusCode.NotFound
                    ? "This playlist could not be found or is not public."
                    : status == HttpStatusCode.Unauthorized || status == HttpStatusCode.Forbidden
                        ? "A valid API key is required to download this playlist."
                        : "The playlist could not be loaded. Please try again later.";
                downloadError?.Invoke(message);
                return;
            }
            catch (Exception exception)
            {
                TwitchBotLogger.Error("Unable to load playlist " + playlistId + ".", exception);
                downloadError?.Invoke("The playlist could not be loaded. Please try again later.");
                return;
            }

            songs = songs ?? new List<SongSearchModel>();
            if (songs.Count == 0)
            {
                downloadTitle?.Invoke("Playlist is empty");
                downloadProgressChanged?.Invoke(100);
                downloadCompleted?.Invoke(autoClose);
                return;
            }

            for (var index = 0; index < songs.Count; index++)
            {
                var songInfo = songs[index];
                var completed = new TaskCompletionSource<bool>();
                try
                {
                    await DownloadAsync(
                        songInfo.Id,
                        progress => downloadProgressChanged?.Invoke(
                            Math.Min(100, (index * 100 + progress) / songs.Count)),
                        _ => completed.TrySetResult(true),
                        title => downloadTitle?.Invoke(
                            (index + 1) + "/" + songs.Count + " " + title),
                        false,
                        null,
                        null);
                    await completed.Task;
                }
                catch (Exception exception)
                {
                    TwitchBotLogger.Error("Unable to download song " + songInfo.Id + " from playlist " + playlistId + ".", exception);
                    downloadError?.Invoke("A song in this playlist could not be downloaded.");
                    return;
                }
            }

            downloadProgressChanged?.Invoke(100);
            downloadTitle?.Invoke("Playlist download complete");
            downloadCompleted?.Invoke(autoClose);
        }
    }
}
