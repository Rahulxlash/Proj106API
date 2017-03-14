using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface ILeagueRepository : IBaseRepository<League>
    { }

    public class LeagueRepository : BaseRepository<League>, ILeagueRepository
    {
        public LeagueRepository(IDatabaseFactory factory) :
            base(factory)
        { }
    }
}
