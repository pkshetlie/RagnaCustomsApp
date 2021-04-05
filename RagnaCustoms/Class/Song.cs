namespace RagnaCustoms
{
    internal class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Mapper { get; set; }
        public string Difficulties { get; set; }
        public string CoverImageExtension { get; set; }
        public string fullTitle
        {
            get
            {
                return $"{Name} - {Author} - {Difficulties}";
            }
        }

    }
}