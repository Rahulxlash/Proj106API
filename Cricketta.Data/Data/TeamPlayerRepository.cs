using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface ITeamPlayerRepository : IBaseRepository<TeamPlayer> { }
    public class TeamPlayerRepository : BaseRepository<TeamPlayer>, ITeamPlayerRepository
    {
        public TeamPlayerRepository(IDatabaseFactory factory) : base(factory) { }
    }
}
