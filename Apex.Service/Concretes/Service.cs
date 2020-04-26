using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;
using Apex.DAL.Abstracts;
using Apex.Service.Abstracts;

namespace Apex.Service.Concretes
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class, IEntity
    {
        public IRepository<TEntity> Repository { get; }
        public RequestInfo Info { get; }
        public IUnitOfWork UnitOfWork { get; }

        //        private IValidator<TEntity> _validator;

        #region Constructor
        public Service(RequestInfo info, IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
            Info = info;
            //            _validator = validator;
        }
        #endregion Constructor


        public List<TEntity> FindAll()
        {
            return List.ToList();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return Repository.Find(keyValues);
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters)
        {
            return Repository.SelectQuery(query, parameters).AsQueryable();
        }

        public virtual void Insert(TEntity entity)
        {
            Repository.Insert(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            Repository.InsertRange(entities);
        }


        public virtual void InsertGraphRange(IEnumerable<TEntity> entities)
        {
            Repository.InsertGraphRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Repository.Update(entity);
        }

        public virtual void UpdateAsync(TEntity entity)
        {
            Repository.Update(entity);
        }

        public virtual void Delete(object id)
        {
            Repository.Delete(id);
        }

        public virtual void Delete(TEntity entity)
        {
            Repository.Delete(entity);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await Repository.FindAsync(keyValues);
        }

        //public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        //{
        //    return await Repository.FindAsync(cancellationToken, keyValues);
        //}

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await Repository.DeleteAsync(keyValues);
        }

        //IF 04/08/2014 - Before: return await DeleteAsync(cancellationToken, keyValues);
        //public virtual async Task<bool> DeleteCtAsync(CancellationToken cancellationToken, params object[] keyValues)
        //{
        //    return await Repository.DeleteCtAsync(cancellationToken, keyValues);
        //}

        public IQueryable<TEntity> List => Repository.Queryable();
    }
}
