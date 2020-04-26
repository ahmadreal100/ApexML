using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apex.Core.Abstract;
using Apex.DAL.Abstracts;

namespace Apex.Service.Abstracts
{
    public interface IService<TEntity> where TEntity : class, IEntity
    {
        IRepository<TEntity> Repository { get; }
        List<TEntity> FindAll();
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void InsertGraphRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateAsync(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        Task<TEntity> FindAsync(params object[] keyValues);
        //Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        //Task<bool> DeleteCtAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryable<TEntity> List { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}
