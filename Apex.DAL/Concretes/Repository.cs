using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;
using Apex.DAL.Abstracts;
using Apex.DAL.EF;
using Apex.Shared.Helpers;
using LinqKit;

namespace Apex.DAL.Concretes
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        #region Protected Fields

        protected readonly DbSet<TEntity> DbSet;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ApexContext Context;
        public RequestInfo Info { get; }

        #endregion Protected Fields

        public Repository(ApexContext context, IUnitOfWork unitOfWork, RequestInfo info)
        {
            Info = info;
            Context = context;
            UnitOfWork = unitOfWork;
            DbSet = context.Set<TEntity>();
            Debug.WriteLine($" contextId: {context.GetHashCode()}");
        }

        public IQueryable<TEntity> Queryable()
        {
            return DbSet.OrderByDescending(x => x.Id);
        }

        public virtual IQueryable<TEntity> Assets
        {
            get
            {
                return DbSet.OrderByDescending(x => x.Id);
            }
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return DbSet.Find(keyValues);
        }

        public virtual void DeleteAll(Expression<Func<TEntity, bool>> filter)
        {
            var all = DbSet.Where(filter);
            DbSet.RemoveRange(all);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return DbSet.SqlQuery(query, parameters).AsQueryable();
        }

        public virtual IEnumerable<TModel> SelectQuery<TModel>(string query, params object[] parameters) where TModel : class
        {
            return Context.Database.SqlQuery<TModel>(query, parameters).AsQueryable();
        }

        public virtual int Exec(string query, params object[] parameters)
        {
            return Context.Database.ExecuteSqlCommand(query, parameters);
        }

        public IQueryable<TEntity> Asset(long id)
        {
            return Assets.Where(m => m.Id == id);
        }

        public TEntity OneAsset(long id, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = Asset(id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query.FirstOrDefault();
        }

        public bool AnyAsset(long id)
        {
            return Asset(id).Any();
        }

        public bool Any(long id)
        {
            return Queryable().Any(m => m.Id == id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Attach(TEntity entity)
        {
            DbSet.Attach(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            entity.ModifiedDate = DateHelper.Now;
        }

        public virtual void UpdateAsync(TEntity entity)
        {
            entity.ModifiedDate = DateHelper.Now;
        }

        public virtual void Delete(object id)
        {
            var entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public IRepository<T> GetRepository<T>() where T : Entity
        {
            return UnitOfWork.Repository<T>();
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await DbSet.FindAsync(keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            var entity = await FindAsync(keyValues);

            if (entity == null)
            {
                return false;
            }

            DbSet.Remove(entity);

            return true;
            //return await DeleteCtAsync(CancellationToken.None, keyValues);
        }

        //public virtual async Task<bool> DeleteCtAsync(CancellationToken cancellationToken, params object[] keyValues)
        //{
        //    var entity = await FindAsync(keyValues);

        //    if (entity == null)
        //    {
        //        return false;
        //    }

        //    DbSet.Remove(entity);

        //    return true;
        //}

        internal IQueryable<TEntity> Select(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query;
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

    }
}
