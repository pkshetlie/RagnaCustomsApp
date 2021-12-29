using System.Collections.Generic;
using System.Linq;
using RagnaCustoms.App.Extensions;

namespace RagnaCustoms.Models
{
    
    public class SearshResult
    {
        public List<Song> Results { get; set; } // list of returned songs
        
        public int Count { get; set; } // number of returned songs

        public Song BestResultByName(string searsh)
        {
            // compare searsh term with song name using Levenshtein distance
            // return the best match
            return Results.OrderBy(x => searsh.ToLower().LevenshteinDistance(x.Name.ToLower())).First();
        }
        public Song FirstResultByName(string searsh)
        {
            return Results.Find(e => e.Name.ToLower().StartsWith(searsh.ToLower()));
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
