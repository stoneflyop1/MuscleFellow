using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MuscleFellow.Data
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(params object[] id);

        Task<T> GetByIdAsync(CancellationToken cancellationToken, params object[] id);

        Task<T> GetByIdAsync(params object[] id);
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        int Insert(T entity);

        Task<int> InsertAsync(T entity);

        Task<int> InsertAsync(T entity, CancellationToken cancellationToken);
        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        int Insert(IEnumerable<T> entities);

        Task<int> InsertAsync(IEnumerable<T> entities);

        Task<int> InsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        int Update(T entity);

        Task<int> UpdateAsync(T entity);

        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken);

        int Update(IEnumerable<T> entities);

        Task<int> UpdateAsync(IEnumerable<T> entities);

        Task<int> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        int Delete(T entity);

        Task<int> DeleteAsync(T entity);

        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken);
        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        int Delete(IEnumerable<T> entities);

        Task<int> DeleteAsync(IEnumerable<T> entities);

        Task<int> DeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Task<T> FirstOrDefaultAsync();

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
        /// <summary>
        /// 包含属性
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        IQueryable<T> Include<TProperty>(Expression<Func<T, TProperty>> path);

        Task<List<T>> ToListAsync();

        Task<List<T>> ToListAsync(CancellationToken cancellationToken);

        Task<T[]> ToArrayAsync();

        Task<T[]> ToArrayAsync(CancellationToken cancellationToken);

        Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector);

        Task<Dictionary<TKey, T>> ToDictionaryAsync<TKey>(Func<T, TKey> keySelector, CancellationToken cancellationToken);

        Task<bool> AnyAsync();

        Task<bool> AnyAsync(CancellationToken cancellationToken);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);

        Task<bool> AllAsync(Expression<Func<T, bool>> predicate);

        Task<bool> AllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    }
}
