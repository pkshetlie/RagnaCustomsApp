using System.Collections.Generic;

namespace RagnaCustoms
{
    internal class SearchResponse
    {
        public List<Song> Results { get; set; }
        public int Count { get;set; }
    }
}