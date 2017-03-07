using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Base
{
    public interface IDatabaseFactory
    {
        CrickContext Get();
    }

    public class DatabaseFactory:IDatabaseFactory, IDisposable
    {
        private CrickContext dbContext;

        public CrickContext Get()
        {
            return dbContext ?? (dbContext = new CrickContext());
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
