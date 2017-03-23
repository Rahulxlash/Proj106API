using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class League
    {
        public int LeagueId { get; set; }
        public String Name { get; set; }
        public int Creator { get; set; }
        public int Competitor { get; set; }
        public DateTime CreateDate { get; set; }
        public int TournamentId { get; set; }
        public int Accepted { get; set; }
        public int Points { get; set; }
        public int CreatorPoint { get; set; }
        public int CompetitorPoint { get; set; }
    }
}
