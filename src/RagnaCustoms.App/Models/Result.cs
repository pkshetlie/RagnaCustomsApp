using System.Collections.Generic;

namespace RagnaCustoms.Models
{
    public class Result<TResult>
    {
        public IEnumerable<TResult> Results { get; set; }
        public int Count { get; set; }
    }
}
