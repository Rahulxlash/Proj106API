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
        public Boolean Accepted { get; set; }
    }
}
