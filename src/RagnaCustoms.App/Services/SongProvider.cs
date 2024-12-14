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

                return searchResult.Results;
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
                    var songInfo = songsInfo.FirstOrDefault(x => x.Id == File.ReadAllText(idFile));
                    if (songInfo != null)
                    {
                        songInfo.CurrentFolder = songpath;
                        if (songInfo.Hash == File.ReadAllText(hashFile)) songInfo.UpToDate = true;
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

            var songDir = $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}";

            downloadTitle?.Invoke($"{songInfo.Name} by {songInfo.Mapper}");
            var songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(),
                songDir);
           
            if (configuration.OrderAlphabetically)
            {
                songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Alphabet");
                if (!Directory.Exists(songDirectoryPath))
                {
                    Directory.CreateDirectory(songDirectoryPath);
                }
                songDirectoryPath = Path.Combine(songDirectoryPath, songInfo.Name.Slug().Substring(0, 1).ToLower());
                if (!Directory.Exists(songDirectoryPath))
                {
                    Directory.CreateDirectory(songDirectoryPath);
                }
                songDirectoryPath = Path.Combine(songDirectoryPath, $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");
            }
            
            if(configuration.OrderMapper){
                songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Mapper");
               
                if (!Directory.Exists(songDirectoryPath))
                {
                    Directory.CreateDirectory(songDirectoryPath);
                }
                
                songDirectoryPath = Path.Combine(songDirectoryPath, songInfo.Mapper.Slug());
                
                if (!Directory.Exists(songDirectoryPath))
                {
                    Directory.CreateDirectory(songDirectoryPath);
                }

                songDirectoryPath = Path.Combine(songDirectoryPath, $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");
            }

            var rankedDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(),"Ranked", songDir);

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

            if(subfolder != null)
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
                if (configuration.CopyRanked && songInfo.IsRanked == "True")
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


                var songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(),
                 $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");
                
                if (configuration.OrderAlphabetically)
                {
                    songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Alphabet");
                    if (!Directory.Exists(songDirectoryPath))
                    {
                        Directory.CreateDirectory(songDirectoryPath);
                    }
                    songDirectoryPath = Path.Combine(songDirectoryPath, songInfo.Name.Slug().Substring(0, 1).ToLower());
                    if (!Directory.Exists(songDirectoryPath))
                    {
                        Directory.CreateDirectory(songDirectoryPath);
                    }
                    songDirectoryPath = Path.Combine(songDirectoryPath, $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");
                }

                if (configuration.OrderMapper)
                {
                    songDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Mapper");
                  
                    if (!Directory.Exists(songDirectoryPath))
                    {
                        Directory.CreateDirectory(songDirectoryPath);
                    }
                  
                    songDirectoryPath = Path.Combine(songDirectoryPath, songInfo.Mapper.Slug());
                   
                    if (!Directory.Exists(songDirectoryPath))
                    {
                        Directory.CreateDirectory(songDirectoryPath);
                    }

                    songDirectoryPath = Path.Combine(songDirectoryPath, $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");
                }

                var rankedDirectoryPath = Path.Combine(DirProvider.getCustomDirectory().ToString(), "Ranked",
                                $"{songInfo.Name.Slug()}{songInfo.Author.Slug()}{songInfo.Mapper.Slug()}");

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

                    if (configuration.CopyRanked && songInfo.IsRanked == "True")
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
                }

            }
        }
    }
}