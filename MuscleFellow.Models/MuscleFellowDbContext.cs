using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuscleFellow.Models.Domain;
using System.Data.Common;
using System.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MuscleFellow.Models
{
    public class MuscleFellowDbContext : IdentityDbContext<IdentityUser>, IDbContext
    {
        private readonly bool _created;

        public MuscleFellowDbContext(DbContextOptions options) :base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Brand>().HasKey(b=>b.BrandID);
            builder.Entity<Category>().HasKey(c=>c.CategoryID);
            builder.Entity<OrderDetail>().HasKey(o=>o.OrderDetailID);
            builder.Entity<Order>().HasKey(o=>o.OrderID);
            builder.Entity<Product>().HasKey(p=>p.ProductID);
            builder.Entity<ShipAddress>().HasKey(a=>a.AddressID);
            builder.Entity<CartItem>().HasKey(c=>c.CartID);
            builder.Entity<ApplicationUser>().HasKey(u=>u.Id);
            builder.Entity<ProductImage>().HasKey(p => p.ImageID);

            base.OnModelCreating(builder);
        }

        public IQueryable<TEntity> Table<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public IQueryable<TEntity> TableNoTracking<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsNoTracking();
        }

        TEntity IDbContext.Attach<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        TEntity IDbContext.Add<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            var list = entities.ToArray();
            base.AddRange(list);
            return list;
        }

        TEntity IDbContext.Remove<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync();
        }

        public EntityEntry<TEntity> Get<TEntity>(TEntity t) where TEntity : class
        {
            return base.Entry(t);
        }

        /// <summary>
        /// 连接一个实体的上下文或返回一个已连接的实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : class, new()
        {
            //little hack here until  实体框架支持存储过程
            //否则, 加载导航属性的实体，是不是装到一个实体连接到上下文
            //Entity必须实现Equals方法
            var alreadyAttached = base.Set<TEntity>().Local.FirstOrDefault(x =>
                EqualityComparer<TEntity>.Default.Equals(x, entity));
            if (alreadyAttached == null)
            {
                //attach new entity
                base.Set<TEntity>().Attach(entity);
                return entity;
            }

            //entity is already loaded
            return alreadyAttached;
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : class
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i <= parameters.Length - 1; i++)
                {
                    var p = parameters[i] as DbParameter;
                    if (p == null)
                        throw new Exception("Not support parameter type");

                    commandText += i == 0 ? " " : ", ";

                    commandText += "@" + p.ParameterName;
                    if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                    {
                        commandText += " output";
                    }
                }
            }

            var result = Set<TEntity>().FromSql(commandText, parameters).ToList();

            //bool acd = this.Configuration.AutoDetectChangesEnabled;
            //try
            //{
            //    this.Configuration.AutoDetectChangesEnabled = false;

            //    for (int i = 0; i < result.Count; i++)
            //        result[i] = AttachEntityToContext(result[i]);
            //}
            //finally
            //{
            //    this.Configuration.AutoDetectChangesEnabled = acd;
            //}

            return result;
        }

        public int ExecuteStoredProcedure(string commandText, params object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                throw new ArgumentException("存储过程必须指定参数", "parameters");
            }
            //add parameters to command
            for (int i = 0; i <= parameters.Length - 1; i++)
            {
                var p = parameters[i] as DbParameter;
                if (p == null)
                    throw new Exception("Not support parameter type");

                commandText += i == 0 ? " " : ", ";

                commandText += "@" + p.ParameterName;
                if (p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                {
                    //output parameter
                    commandText += " output";
                }
            }
            var returnStr = 0;
            try
            {
                var result = this.Database.ExecuteSqlCommand(commandText, parameters);
                returnStr = Convert.ToInt32(((DbParameter)parameters[parameters.Length - 1]).Value);
            }
            catch (Exception)
            {

            }
            return returnStr;
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters) where TElement : class
        {
            return Set<TElement>().FromSql(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                previousTimeout = Database.GetCommandTimeout();
                Database.SetCommandTimeout(timeout);
            }

            if (!doNotEnsureTransaction)
            {
                if (Database.CurrentTransaction == null)
                {
                    Database.BeginTransaction();
                }
            }

            var result = Database.ExecuteSqlCommand(sql, parameters);

            if (timeout.HasValue)
            {
                Database.SetCommandTimeout(previousTimeout);
            }

            return result;
        }

        //public DbSet<Brand> Brands { get; set; }

        //public DbSet<Category> Categories { get; set; }

        //public DbSet<Order> Orders { get; set; }

        //public DbSet<OrderDetail> OrderDetails { get; set; }

        //public DbSet<Product> Products { get; set; }

        //public DbSet<CartItem> CartItems { get; set; }

        //public DbSet<ShipAddress> ShipAddresses { get; set; }

        //public DbSet<ProductImage> ProductImages { get; set; }

        //public DbSet<Province> Provinces { get; set; }

        //public DbSet<City> Cities { get; set; }

    }
}
