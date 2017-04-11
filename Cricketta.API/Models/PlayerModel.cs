using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricketta.API.Models
{
    public class PlayerModel
    {
        public int PlayerId { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public bool isExtra { get; set; }
        public bool isPlaying { get; set; }
        public int Run { get; set; }
        public int Wicket { get; set; }
        public string Name { get; set; }
        public Boolean Bat { get; set; }
        public Boolean Bowl { get; set; }
        public Boolean Keeper { get; set; }
        public Boolean Captain { get; set; }
        public String Photo { get; set; }
    }
}