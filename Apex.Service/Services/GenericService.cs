using System.Linq;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;
using Apex.DAL.Abstracts;
using Apex.Service.Services.Base;
using AutoMapper;

namespace Apex.Service.Services
{
    public class GenericService<T, TVm> : BaseService where T : class, IEntity, new() where TVm : class
    {
        public IRepository<T> Repository { get; }
        public GenericService(IUnitOfWork unitOfWork,
            RequestInfo info,
            IRepository<T> repository)
            : base(unitOfWork, info)
        {
            Repository = repository;
        }

        public async Task<ServiceResult<TVm>> Create(TVm model)
        {
            var res = new ServiceResult<TVm>();

            var en = Mapper.Map<T>(model);

            Repository.Insert(en);

            await UnitOfWork.SaveChangesAsync();

            res.Result = Mapper.Map<TVm>(en);
            return res;
        }

        public async Task<ServiceResult<TVm>> Update(long id, TVm model)
        {
            var res = new ServiceResult<TVm>();

            var en = Repository.OneAsset(id);
            if (en == null)
            {
                res.NotFound = true;
                return res;
            }

            Mapper.Map(model, en);

            Repository.Update(en);

            await UnitOfWork.SaveChangesAsync();

            return res;
        }

        public async Task<ServiceResult<TVm>> AddOrUpdate(TVm model)
        {
            var res = new ServiceResult<TVm>();

            var en = Repository.Assets.FirstOrDefault() ?? new T();

            Mapper.Map(model, en);
            if (en.Id > 0)
                Repository.Update(en);
            else
            {
                Repository.Insert(en);
            }

            await UnitOfWork.SaveChangesAsync();

            return res;
        }

        public async Task<ServiceResult<TVm>> Delete(long id)
        {
            var res = new ServiceResult<TVm>();

            var en = Repository.OneAsset(id);
            if (en == null)
            {
                res.NotFound = true;
                return res;
            }


            Repository.Delete(en);

            await UnitOfWork.SaveChangesAsync();

            return res;
        }
    }
}
