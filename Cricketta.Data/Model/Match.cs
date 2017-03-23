using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    [Table("Match")]
    public class Match
    {
        public int MatchId { get; set; }
        public int TournamentId { get; set; }
        public int TeamId1 { get; set; }
        public int TeamId2 { get; set; }
        public String Type { get; set; }
        public String Venue { get; set; }
        public DateTime MatchDate { get; set; }

    }
}
