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
        public int Competitor { get; set; }
        public DateTime CreateDate { get; set; }
        public Boolean Accepted { get; set; }
    }
}