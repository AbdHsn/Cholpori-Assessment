using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryLayer
{
    public interface IEntityRepo<T> where T: class
    {
        #region "Get Methods Definition"
        IEnumerable<T> GetAll();
        T GetById(Expression<Func<T, bool>> predicate);
        #endregion "Get Methods Definition"

        #region "DB Operation Methods Definition"
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        #endregion "DB Operation Methods Definition"
    }
}
