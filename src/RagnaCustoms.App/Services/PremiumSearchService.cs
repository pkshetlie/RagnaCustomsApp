using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using DrawingBitmap = System.Drawing.Bitmap;
using DrawingImage = System.Drawing.Image;

namespace RagnaCustoms.Services
{
    public sealed class PremiumSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public int SongCount { get; set; }
    }

    public sealed class PremiumSearchPage
    {
        public int Total { get; set; }
        public List<PremiumSearchResult> Results { get; set; }
    }

    public sealed class PremiumSongResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Mapper { get; set; }
        public bool IsRanked { get; set; }
        public string CoverUrl { get; set; }

        public override string ToString()
        {
            var artist = string.IsNullOrWhiteSpace(Artist) ? string.Empty : " - " + Artist;
            var mapper = string.IsNullOrWhiteSpace(Mapper) ? string.Empty : " - " + Mapper;
            var ranked = IsRanked ? " [Ranked]" : string.Empty;
            return Name + artist + mapper + ranked;
        }
    }

    public sealed class PremiumSongDetails
    {
        public string Artist { get; set; }
        public string Mapper { get; set; }
        public string CoverUrl { get; set; }
    }

    public sealed class PremiumSearchException : Exception
    {
        public PremiumSearchException(HttpStatusCode statusCode, string detail)
            : base("Premium search failed with HTTP " + (int)statusCode + ".")
        {
            StatusCode = statusCode;
            Detail = detail;
        }

        public HttpStatusCode StatusCode { get; private set; }
        public string Detail { get; private set; }
    }

    public sealed class PremiumSearchService
    {
        private const string ApiBaseUrl = "https://api.ragnacustoms.com/api";
        private readonly string _apiKey;

        public PremiumSearchService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public Task<PremiumSearchPage> SearchPlaylistsAsync(string query)
        {
            return SearchAsync("playlist/search", query);
        }

        public Task<PremiumSearchPage> SearchArtistsAsync(string query)
        {
            return SearchAsync("artist/search", query);
        }

        public async Task<List<PremiumSongResult>> GetPlaylistSongsAsync(string playlistId)
        {
            var payload = await GetJsonAsync("playlist/" + Uri.EscapeDataString(playlistId));
            var songs = payload["songs"] as JArray;
            return ParseSongs(songs);
        }

        public async Task<List<PremiumSongResult>> GetArtistSongsAsync(string artistId)
        {
            var songs = new List<PremiumSongResult>();
            const int pageSize = 50;
            var page = 1;

            while (true)
            {
                var payload = await GetJsonAsync(
                    "artist/" + Uri.EscapeDataString(artistId) +
                    "/songs?page=" + page + "&pageSize=" + pageSize);
                var pageResults = ParseSongs(payload["results"] as JArray);
                songs.AddRange(pageResults);

                var total = payload["total"]?.Value<int>() ?? songs.Count;
                if (pageResults.Count == 0 || songs.Count >= total || pageResults.Count < pageSize)
                {
                    return songs;
                }

                page++;
            }
        }

        public async Task<PremiumSongDetails> GetSongDetailsAsync(string songId)
        {
            var payload = await GetJsonAsync("song/details/" + Uri.EscapeDataString(songId));
            return new PremiumSongDetails
            {
                Artist = ReadNestedString(payload["author"], "fullname", "name")
                    ?? ReadString(payload, "author", "Author"),
                Mapper = ReadNestedString(payload["mapper"], "fullname", "name")
                    ?? ReadString(payload, "mapper", "Mapper"),
                CoverUrl = ReadString(payload, "coverUrl", "CoverUrl")
                    ?? ReadString(payload, "cover", "Cover")
            };
        }

        public async Task<DrawingBitmap> DownloadCoverAsync(string coverUrl)
        {
            if (string.IsNullOrWhiteSpace(coverUrl)) return null;

            var uris = new List<Uri>();
            Uri absoluteUri;
            if (Uri.TryCreate(coverUrl, UriKind.Absolute, out absoluteUri))
            {
                uris.Add(absoluteUri);

                if (string.Equals(absoluteUri.Host, "ragnacustoms.com", StringComparison.OrdinalIgnoreCase))
                {
                    uris.Insert(0, new UriBuilder(absoluteUri)
                    {
                        Host = "api.ragnacustoms.com"
                    }.Uri);
                }
                else if (string.Equals(absoluteUri.Host, "api.ragnacustoms.com", StringComparison.OrdinalIgnoreCase))
                {
                    uris.Add(new UriBuilder(absoluteUri)
                    {
                        Host = "ragnacustoms.com"
                    }.Uri);
                }
            }
            else
            {
                uris.Add(new Uri(new Uri("https://api.ragnacustoms.com"), coverUrl));
                uris.Add(new Uri(new Uri("https://ragnacustoms.com"), coverUrl));
            }

            foreach (var uri in uris)
            {
                try
                {
                    using (var client = new HttpClient())
                    using (var response = await client.GetAsync(uri))
                    {
                        if (!response.IsSuccessStatusCode) continue;

                        var bytes = await response.Content.ReadAsByteArrayAsync();
                        using (var bitmap = SKBitmap.Decode(bytes))
                        using (var image = SKImage.FromBitmap(bitmap))
                        using (var encodedImage = image.Encode(SKEncodedImageFormat.Png, 100))
                        using (var pngStream = new MemoryStream())
                        {
                            encodedImage.SaveTo(pngStream);
                            pngStream.Position = 0;

                            using (var drawingImage = DrawingImage.FromStream(pngStream))
                            {
                                return new DrawingBitmap(drawingImage);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    TwitchBotLogger.Error("Unable to download cover from " + uri + ".", exception);
                }
            }

            return null;
        }

        private async Task<PremiumSearchPage> SearchAsync(string endpoint, string query)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new PremiumSearchException(HttpStatusCode.Unauthorized, "A valid API key is required.");
            }

            var uri = new Uri(ApiBaseUrl + "/" + endpoint +
                "?q=" + Uri.EscapeDataString(query.Trim()) + "&page=1&pageSize=20");
            var payload = await GetJsonAsync(uri);
            return ParsePage(payload.ToString());
        }

        private async Task<JObject> GetJsonAsync(string endpoint)
        {
            return await GetJsonAsync(new Uri(ApiBaseUrl + "/" + endpoint));
        }

        private async Task<JObject> GetJsonAsync(Uri uri)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new PremiumSearchException(HttpStatusCode.Unauthorized, "A valid API key is required.");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
                using (var response = await client.GetAsync(uri))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new PremiumSearchException(response.StatusCode, ExtractDetail(content));
                    }

                    try
                    {
                        return JObject.Parse(content);
                    }
                    catch (Exception exception)
                    {
                        throw new PremiumSearchException(response.StatusCode,
                            "The API returned an invalid search response: " + exception.Message);
                    }
                }
            }
        }

        private static PremiumSearchPage ParsePage(string content)
        {
            var payload = JObject.Parse(content);
            var page = new PremiumSearchPage
            {
                Total = payload["total"]?.Value<int>() ?? 0,
                Results = new List<PremiumSearchResult>()
            };

            var results = payload["results"] as JArray;
            if (results == null) return page;

            foreach (var item in results)
            {
                page.Results.Add(new PremiumSearchResult
                {
                    Id = ReadString(item, "id", "Id"),
                    Name = ReadString(item, "name", "Name"),
                    Owner = ReadString(item, "owner", "Owner"),
                    SongCount = ReadInt(item, "songCount", "SongCount")
                });
            }

            return page;
        }

        private static List<PremiumSongResult> ParseSongs(JArray songs)
        {
            var results = new List<PremiumSongResult>();
            if (songs == null) return results;

            foreach (var item in songs)
            {
                results.Add(new PremiumSongResult
                {
                    Id = ReadString(item, "id", "Id"),
                    Name = ReadString(item, "name", "Name"),
                    Artist = ReadString(item, "artist", "Artist")
                        ?? ReadString(item, "author", "Author"),
                    Mapper = ReadString(item, "mapper", "Mapper"),
                    IsRanked = ReadBool(item, "isRanked", "IsRanked"),
                    CoverUrl = ReadString(item, "coverUrl", "CoverUrl")
                });
            }

            return results;
        }

        private static string ReadString(JToken token, string firstName, string secondName)
        {
            return token[firstName]?.ToString() ?? token[secondName]?.ToString();
        }

        private static string ReadNestedString(JToken token, string firstName, string secondName)
        {
            if (token == null || token.Type != JTokenType.Object) return null;
            return ReadString(token, firstName, secondName);
        }

        private static int ReadInt(JToken token, string firstName, string secondName)
        {
            return token[firstName]?.Value<int>() ?? token[secondName]?.Value<int>() ?? 0;
        }

        private static bool ReadBool(JToken token, string firstName, string secondName)
        {
            return token[firstName]?.Value<bool>() ?? token[secondName]?.Value<bool>() ?? false;
        }

        private static string ExtractDetail(string content)
        {
            try
            {
                return JObject.Parse(content)["detail"]?.ToString() ?? content;
            }
            catch
            {
                return content;
            }
        }
    }
}
