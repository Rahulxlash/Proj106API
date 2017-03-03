using Cricketta.Data.Base;
using Cricketta.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Data
{
    public interface IUserRepository : IBaseRepository<User>
    {

    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IDatabaseFactory factory)
            : base(factory)
        { }
    }
}
