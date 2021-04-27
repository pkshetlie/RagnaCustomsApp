using System.Collections.Generic;

namespace RagnaCustoms.Models
{
    public class SearchResult<TResult>
    {
        public IEnumerable<TResult> Results { get; set; }
        public int Count { get; set; }
    }
}
