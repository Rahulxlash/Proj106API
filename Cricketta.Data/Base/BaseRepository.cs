using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cricketta.Data.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private CrickContext dbContext;
        private readonly IDbSet<T> dbSet;

        protected IDatabaseFactory dbFactory { get; private set; }

        protected BaseRepository(IDatabaseFactory factory)
        {
            this.dbFactory = factory;
            dbSet = this.DataContext.Set<T>();
        }

        protected CrickContext DataContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Get()); }
        }

        public virtual T Add(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
                dbSet.Remove(obj);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).AsEnumerable();
        }

        public virtual T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }
    }
}
