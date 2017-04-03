using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface IPlayerRepository: IBaseRepository<Player>
    {

    }
    public class PlayerRepository: BaseRepository<Player> , IPlayerRepository
    {
        public PlayerRepository(IDatabaseFactory factory):base(factory){}
    }
}
