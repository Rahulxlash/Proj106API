using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Base
{
    public interface IUnitofWork
    {
        void SaveChanges();
    }
    public class UnitofWork:IUnitofWork
    {
        private CrickContext context;
        private IDatabaseFactory databaseFactory;

        protected CrickContext DbContext
        {
            get
            {
                return context ?? databaseFactory.Get();
            }
        }

        public UnitofWork(IDatabaseFactory factory)
        {
            this.databaseFactory = factory;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }
    }
}
