namespace RagnaCustoms.Models
{
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
