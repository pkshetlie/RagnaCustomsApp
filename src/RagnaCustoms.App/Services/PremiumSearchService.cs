using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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

        private async Task<PremiumSearchPage> SearchAsync(string endpoint, string query)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new PremiumSearchException(HttpStatusCode.Unauthorized, "A valid API key is required.");
            }

            var uri = new Uri(ApiBaseUrl + "/" + endpoint +
                "?q=" + Uri.EscapeDataString(query.Trim()) + "&page=1&pageSize=20");
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
                        return ParsePage(content);
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
                    Id = item["id"]?.ToString(),
                    Name = item["name"]?.ToString() ?? string.Empty,
                    Owner = item["owner"]?.ToString() ?? string.Empty,
                    SongCount = item["songCount"]?.Value<int>() ?? 0
                });
            }

            return page;
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
