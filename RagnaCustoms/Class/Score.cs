using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RagnaCustoms.Class
{
    public class Score
    {
        public Score()
        {
            Scores = new List<SubScore>();
        }
        public string ApiKey { get; set; }
        public List<SubScore> Scores { get; set; }


    }
    public class SubScore
    {
        public string HashInfo { get; set; }
        public string Score { get; set; }
        public string Level { get; set; }
    }
}
