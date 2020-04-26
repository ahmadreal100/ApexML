using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;

namespace Apex.DAL.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : Entity;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();

        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}