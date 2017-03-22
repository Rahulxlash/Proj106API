using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricketta.API.Models
{
    public class LeagueModel
    {
        public int LeagueId { get; set; }
        public String Name { get; set; }
        public int Creator { get; set; }
        public string Competitor { get; set; }
        public DateTime CreateDate { get; set; }
        public int Accepted { get; set; }
        public String CompetitorName { get; set; }
        public bool IsMyLeague { get; set; }
        public int Points { get; set; }
    }
}