using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class PlayerScoreCard
    {
        public int RecordId { get; set; }
        public int TournamentId { get; set; }
        public int MatchId { get; set; }
        public int PlayerId { get; set; }
        public int Run { get; set; }
        public int Wicket { get; set; }
    }
}
