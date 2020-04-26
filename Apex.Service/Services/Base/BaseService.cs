using Apex.Core;
using Apex.Core.Entities.LocaleE;
using Apex.DAL.Abstracts;
using Apex.DAL.Helpers;

namespace Apex.Service.Services.Base
{
    public class BaseService
    {
        public BaseService(IUnitOfWork unitOfWork, RequestInfo info)
        {
            UnitOfWork = unitOfWork;
            Info = info;
        }

        public IUnitOfWork UnitOfWork { get; }
        public RequestInfo Info { get; }
        public Language CurrentLanguage => CultureHelper.CurrentLanguage();
    }
}
