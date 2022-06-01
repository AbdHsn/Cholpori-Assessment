using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RepositoryLayer
{
    public class EntityRepo<T> : IEntityRepo<T> where T : class
    {
        #region "Variables"
        private readonly EntityContext _context;
        private DbSet<T> entities;
        #endregion "Variables"

        #region "Constructors"
        public EntityRepo(EntityContext context)
        {
            this._context = context;
            entities = _context.Set<T>();
        }

        #endregion "Constructors"

        #region "Get Methods Implementation"
        public IEnumerable<T> GetAll()
        {
            return entities;
        }
       
        public T GetById(Expression<Func<T, bool>> predicate)
        {
            return entities.FirstOrDefault(predicate);
        }
        #endregion "Get Methods Implementation"

        #region "DB Operation Methods Implementation"

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Please provide all information correctly");
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Please provide all information correctly");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Delete is not successful");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }
        #endregion "DB Operation Methods Implementation"

        #region "Get Methods Implementation using RawSQL"

        public IQueryable<T> GetAllByRawSql(string sqlQuery)
        {
            return null;//_context.Set<T>().FromSqlRaw("");
        }
        #endregion "Get Methods Implementation using RawSQL"
    }
}