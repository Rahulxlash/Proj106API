using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface IMatchRepository : IBaseRepository<Match>
    { }

    public class MatchRepository : BaseRepository<Match>, IMatchRepository
    {
        public MatchRepository(IDatabaseFactory factory)
            : base(factory)
        { }
    }
}
