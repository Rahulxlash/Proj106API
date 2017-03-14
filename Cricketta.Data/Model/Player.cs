using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Model
{
    public class Player 
    {
        public int PlayerId { get; set; }
        public String Name { get; set; }
        public Boolean Bat { get; set; }
        public Boolean Bowl { get; set; }
        public Boolean Keeper { get; set; }
        public Boolean Captain { get; set; }
        public int Age { get; set; }
        public String Photo { get; set; }
    }
}
