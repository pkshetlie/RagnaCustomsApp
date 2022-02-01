using System;
using System.Collections.Generic;
using System.Linq;
using RagnaCustoms.App.Extensions;

namespace RagnaCustoms.Models
{
    
    public class SearshResult
    {
        public List<Song> Results { get; set; } // list of returned songs
        
        public int Count { get; set; } // number of returned songs

        public Song BestResultByName(string search)
        {
            // compare searsh term with song name using Levenshtein distance
            // return the best match
            return Results.OrderBy(x => search.ToLower().LevenshteinDistance(x.Name.ToLower())).FirstOrDefault();
        }
        public Song FirstResultByName(string search)
        {
            return Results.Find(e => e.Name.ToLower().StartsWith(search.ToLower()));
        }
        
    }
    
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public string Hash { get; set; }
        public string Requester { get; set; } = null;
        
        public override string ToString()
        {
            return $"{Name} {Level} by {Author}";
        }
    }
    
    public class SongSearchModel
    {
        public int Id { get; set; }
        public bool UpToDate { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string IsRanked { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public string Difficulties { get; set; }
    }
}
