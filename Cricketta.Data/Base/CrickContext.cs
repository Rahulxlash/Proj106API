using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Base
{
    public class CrickContext : DbContext
    {
        public CrickContext()
            : base("CrickContext")
        {
            Database.SetInitializer<CrickContext>(null);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<League> Leagues { get; set; }
    }
}
