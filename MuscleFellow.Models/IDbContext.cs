using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MuscleFellow.Models
{
    public interface IDbContext : IDisposable
    {
        TEntity Find<TEntity>(params object[] id) where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(params object[] id) where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking<TEntity>() where TEntity : class;

        TEntity Attach<TEntity>(TEntity entity) where TEntity : class;
        void AttachRange(IEnumerable<object> entities);
        void AttachRange(params object[] entities);

        TEntity Add<TEntity>(TEntity entity) where TEntity : class;

        void AddRange(IEnumerable<object> entities);
        void AddRange(params object[] entities);
        Task AddRangeAsync(params object[] entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default(CancellationToken));
        IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        TEntity Remove<TEntity>(TEntity entity) where TEntity : class;

        void RemoveRange(IEnumerable<object> entities);
        void RemoveRange(params object[] entities);
        IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        void UpdateRange(params object[] entities);
        void UpdateRange(IEnumerable<object> entities);
        ///// <summary>
        ///// 获取 DbSet
        ///// </summary>
        ///// <typeparam name="TEntity">实体类型</typeparam>
        ///// <returns>DbSet</returns>
        //IDbSet<TEntity> Entities<TEntity>() where TEntity : class;

        /// <summary>
        /// 保存更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        /// <summary>
        /// 存储过程获取数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="commandText">Sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>实体</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class;
        int ExecuteStoredProcedure(string commandText, params object[] parameters);
        /// <summary>
        /// Sql语句查询
        /// </summary>
        /// <typeparam name="TElement">类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class;

        /// <summary>
        /// Sql命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="doNotEnsureTransaction">事务</param>
        /// <param name="timeout">超时时间(s)</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回影响的行数</returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        //DbEntityEntry<TEntity> Get<TEntity>(TEntity t) where TEntity : class;

        //DbContextConfiguration Configuration { get; }
    }
}
