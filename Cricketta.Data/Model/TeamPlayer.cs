using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class TeamPlayer
    {
        public int TeamPlayerId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
    }
}
