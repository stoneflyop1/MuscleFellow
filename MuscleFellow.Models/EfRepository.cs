using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleFellow.Models;

namespace MuscleFellow.Data
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public class EfRepository<T> : IRepository<T> where T : class
    {
        #region Fields

        private readonly IDbContext _context;
        //private IDbSet<T> _entities;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(params object[] id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return _context.Find<T>(id);
        }

        public Task<T> GetByIdAsync(params object[] id)
        {
            return _context.FindAsync<T>(id);
        }
        public virtual Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] id)
        {
            return _context.FindAsync<T>(cancellationToken, id);
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Add(entity);
            return SaveChanges();
        }

        public virtual Task<int> InsertAsync(T entity)
        {
            return InsertAsync(entity, CancellationToken.None);
        }

        public Task<int> InsertAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Add(entity);

            return SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Insert(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            AddEntites(entities);

            return SaveChanges();

        }

        private void AddEntites(IEnumerable<T> entities)
        {
            _context.AddRange(entities);
        }

        public virtual Task<int> InsertAsync(IEnumerable<T> entities)
        {
            return InsertAsync(entities, CancellationToken.None);
        }

        public virtual Task<int> InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            AddEntites(entities);

            return SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return Update();
        }

        public virtual Task<int> UpdateAsync(T entity)
        {
            return UpdateAsync(entity, CancellationToken.None);
        }

        public virtual Task<int> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return UpdateAsync(cancellationToken);
        }

        private int Update()
        {
            return SaveChanges();
        }

        private Task<int> UpdateAsync(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(cancellationToken);
        }

        public int Update(IEnumerable<T> entities)
        {
            return Update();
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            return UpdateAsync(CancellationToken.None);
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            return UpdateAsync(cancellationToken);
        }
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Remove(entity);

            return SaveChanges();
        }

        public virtual Task<int> DeleteAsync(T entity)
        {
            return DeleteAsync(entity, CancellationToken.None);
        }

        public virtual Task<int> DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _context.Remove(entity);
            return SaveChangesAsync(cancellationToken);
        }

        private void RemoveEntites(IEnumerable<T> entities)
        {
            _context.RemoveRange(entities);
        }
        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Delete(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            RemoveEntites(entities);
            return SaveChanges();
        }

        public virtual Task<int> DeleteAsync(IEnumerable<T> entities)
        {
            return DeleteAsync(entities, CancellationToken.None);
        }

        public virtual Task<int> DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            RemoveEntites(entities);

            return SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return _context.Table<T>();
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get { return _context.TableNoTracking<T>(); }
        }

        ///// <summary>
        ///// Entities
        ///// </summary>
        //protected virtual IDbSet<T> Entities
        //{
        //    get { return _entities ?? (_entities = _context.Entities<T>()); }
        //}

        #endregion


        public Task<T> FirstOrDefaultAsync()
        {
            return Table.FirstOrDefaultAsync();
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return Table.FirstOrDefaultAsync(predicate);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return Table.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> path)
        {
            return Table.Include(path);
        }

        public Task<List<T>> ToListAsync()
        {
            return Table.ToListAsync();
        }

        public Task<List<T>> ToListAsync(CancellationToken cancellationToken)
        {
            return Table.ToListAsync(cancellationToken);
        }

        public Task<T[]> ToArrayAsync()
        {
            return Table.ToArrayAsync();
        }

        public Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            return Table.ToArrayAsync(cancellationToken);
        }

        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector)
        {
            return Table.ToDictionaryAsync<T, TKey>(keySelector);
        }

        public Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, CancellationToken cancellationToken)
        {
            return Table.ToDictionaryAsync<T, TKey>(keySelector, cancellationToken);
        }

        public Task<bool> AnyAsync()
        {
            return Table.AnyAsync();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken)
        {
            return Table.AnyAsync(cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return Table.AnyAsync(predicate);
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return Table.AnyAsync(predicate, cancellationToken);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate)
        {
            return Table.AllAsync(predicate);
        }

        public Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return Table.AllAsync(predicate, cancellationToken);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
