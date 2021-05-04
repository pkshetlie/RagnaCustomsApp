namespace RagnaCustoms.Models
{
    public class Session
    {
        public Song Song { get; set; }
        public string Score { get; set; }

        public Session()
        {
            Song = new Song();
        }

        public override string ToString()
        {
            return $"{Song}: {Score}";
        }
    }
}
