namespace RagnaCustoms.Models
{
    public class Session
    {
        public Session()
        {
            Song = new Song();
        }

        public Song Song { get; set; }
        public string Score { get; set; }
        public int NotesMissed { get; set; }
        public int NotesHit { get; set; }
        public int NotesNotProcessed { get; set; }
        public string HitAccuracy { get; set; }
        public string Percentage { get; set; }
        public string HitSpeed { get; set; }
        public string Percentage2 { get; set; }
        public int Combos { get; set; }

        public override string ToString()
        {
            return $"{Song}: {Score}";
        }
    }
}