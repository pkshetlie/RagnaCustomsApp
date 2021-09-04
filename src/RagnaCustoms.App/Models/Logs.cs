namespace RagnaCustoms.Models
{
    public class Logs
    {
        private string _dateLogs;
        private string _songNameLogs;
        private string _difficultyLogs;
        private string _scoreLogs;
        private string _statusLogs;
        private string _hashLogs;

        public string DateLogs { get => _dateLogs; set => _dateLogs = value; }
        public string SongNameLogs { get => _songNameLogs; set => _songNameLogs = value; }
        public string DifficultyLogs { get => _difficultyLogs; set => _difficultyLogs = value; }
        public string ScoreLogs { get => _scoreLogs; set => _scoreLogs = value; }
        public string StatusLogs { get => _statusLogs; set => _statusLogs = value; }
        public string HashLogs { get => _hashLogs; set => _hashLogs = value; }

        public Logs()
        {
        }
    }
}
