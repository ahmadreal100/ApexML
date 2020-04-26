using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apex.Core.Entities.UserE;
using Apex.DAL.Abstracts;
using Apex.Shared.Helpers;

namespace Apex.DAL.Helpers
{
    public static class RequestHelper
    {
        public static HttpRequest Request => HttpContext.Current.Request;
        public static HttpContext Context => HttpContext.Current;
        public static IUserRepository UserRepository => (IUserRepository)DependencyResolver.Current.GetService(typeof(IUserRepository));

        public static MasterInfo GetMasterInfo()
        {
            return GetMasterUser()?.MasterInfo;

        }
        public static User GetMasterUser()
        {
            return UserRepository.Queryable()
                .Include(x => x.MasterInfo.City)
                .Include(x => x.MasterInfo.Province)
                .Include(x => x.MasterInfo.Translations)
                .FirstOrDefault(x => x.UserName == AppConst.Auth.Admin);

        }


        public static string GetUrl(this HttpRequestBase request)
        {
            var c = request.RequestContext.RouteData.Values["controller"];
            var a = request.RequestContext.RouteData.Values["action"];
            return $"/{c}/{a}";
        }


    }
}