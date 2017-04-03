using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface ILeagueScoreCardRepository : IBaseRepository<LeagueScoreCard> { }
    public class LeagueScoreCardRepository: BaseRepository<LeagueScoreCard>, ILeagueScoreCardRepository
    {
        public LeagueScoreCardRepository(IDatabaseFactory factory) : base(factory) { }
    }
}
