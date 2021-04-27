using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<Song>> SearchOnlineAsync(string term)
        {
            using var client = new HttpClient();

            var uri = new Uri($"https://ragnacustoms.com/api/search/{term}");
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var searchResult = JsonSerializer.Deserialize<SearchResult<Song>>(content);

                return searchResult.Results;
            }

            return Enumerable.Empty<Song>();
        }

        public async Task DownloadAsync(int songId)
        {
            using var client = new WebClient();

            var uri = new Uri($"https://ragnacustoms.com/songs/download/{songId}");
            var tempFilePath = Path.GetTempFileName();

            await client.DownloadFileTaskAsync(uri, tempFilePath);

            ZipFile.ExtractToDirectory(tempFilePath, RagnarockSongDirectoryPath);
            File.Delete(tempFilePath);
        }

        protected virtual IEnumerable<FileInfo> GetLocalFiles() => RagnarockSongDirectory.EnumerateFiles(SongSearchPattern);
        protected virtual IEnumerable<Song> GetLocalSongs() => GetLocalFiles().Select(file => new Song { Name = file.Name }).OrderBy(song => song.Name);
    }
}
