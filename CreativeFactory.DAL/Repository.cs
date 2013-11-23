using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;

namespace CreativeFactory.DAL
{
    /// <summary>
    /// Defines class for CRUD operations for entity.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public class Repository<T> where T : class
    {
        internal readonly CreativeFactoryContext _context;
        internal readonly DbSet<T> _dbSet;

        public Repository(CreativeFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Read an entity from database.
        /// </summary>
        /// <param name="id">Entity ID.</param>
        /// <returns>Entity.</returns>
        public virtual T GetByID(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Get all entities of type <typeparamref name="T"/> from database.
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns>Collection of entities.</returns>
        public virtual IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;
            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        /// <summary>
        /// Insert entity into database.
        /// </summary>
        /// <param name="entity">Entity to insert.</param>
        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Update entity in database.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete entity from database by its ID.
        /// </summary>
        /// <param name="id">Entity ID.</param>
        public virtual void Delete(int id)
        {
            T entity = _dbSet.Find(id);
            Delete(entity);
        }

        /// <summary>
        /// Delete entity from database.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public virtual void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
    }
}
