using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class LeagueMatch
    {
        public int LeagueMatchId { get; set; }
        public int MatchId { get; set; }
        public int TeamId1 { get; set; }
        public int TeamId2 { get; set; }
        public int LeagueId { get; set; }
        public bool TossDone { get; set; }
        public int Toss { get; set; } //0=Toss pending, -1 = request pending, 1 = creator, 2 = competitor
        public int CreatorRun { get; set; }
        public int CreatorWicket { get; set; }
        public int CreatorPoint { get; set; }
        public int CompetitorRun { get; set; }
        public int CompetitorWicket { get; set; }
        public int CompetitorPoint { get; set; }
        public DateTime MatchDate { get; set; }
        public String Venue { get; set; }
        public string TeamName1 { get; set; }
        public string TeamName2 { get; set; }
    }
}
