using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class Tournament
    {
        public int TournamentId { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public String Logo { get; set; }

    }
}
