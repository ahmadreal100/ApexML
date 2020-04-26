using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;

namespace Apex.DAL.Abstracts
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        RequestInfo Info { get; }
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> Queryable();

        IQueryable<TEntity> Assets { get; }
        IQueryable<TEntity> Asset(long id);
        TEntity OneAsset(long id, params Expression<Func<TEntity, object>>[] includes);
        bool AnyAsset(long id);

        bool Any(long id);

        void Insert(TEntity entity);
        void Attach(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateAsync(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        IRepository<T> GetRepository<T>() where T : Entity;
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        void DeleteAll(Expression<Func<TEntity, bool>> filter);
        //Task<bool> DeleteCtAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        IEnumerable<TModel> SelectQuery<TModel>(string query, params object[] parameters) where TModel : class;
        int Exec(string query, params object[] parameters);
        Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null);
    }
}
