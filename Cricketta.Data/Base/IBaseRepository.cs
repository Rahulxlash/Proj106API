using System;
using System.Threading.Tasks;
namespace Cricketta.Data.Base
{
    public interface IBaseRepository<T>
     where T : class
    {
        T Add(T entity);
        void Delete(System.Linq.Expressions.Expression<Func<T, bool>> where);
        void Delete(T entity);
        System.Collections.Generic.IEnumerable<T> GetAll();
        T GetById(int id);
        System.Collections.Generic.IEnumerable<T> GetMany(System.Linq.Expressions.Expression<Func<T, bool>> where);
        System.Linq.IQueryable<T> Query(System.Linq.Expressions.Expression<Func<T, bool>> where);
        void Update(T entity);
    }
}
