namespace RagnaCustoms.Models
{
    public class Logs
    {
        private string dateLogs;
        private string songNameLogs;
        private string difficultyLogs;
        private string scoreLogs;
        private string statusLogs;
        private string hashLogs;

        public string DateLogs { get => dateLogs; set => dateLogs = value; }
        public string SongNameLogs { get => songNameLogs; set => songNameLogs = value; }
        public string DifficultyLogs { get => difficultyLogs; set => difficultyLogs = value; }
        public string ScoreLogs { get => scoreLogs; set => scoreLogs = value; }
        public string StatusLogs { get => statusLogs; set => statusLogs = value; }
        public string HashLogs { get => hashLogs; set => hashLogs = value; }

        public Logs()
        {
        }
    }
}
