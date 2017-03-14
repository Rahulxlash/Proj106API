using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class TournamentTeam
    {
        public int TournamentTeamId { get; set; }

        public int TournamentId { get; set; }

        public int TeamId { get; set; }
    }
}
