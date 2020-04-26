using System.Security.Claims;
using System.Security.Principal;
using Apex.Core.Enums;
using Apex.Shared.Helpers;
using Microsoft.AspNet.Identity;

namespace Apex.Service.Extensions
{
    public static class PrincipalExtension
    {
        public static bool IsAdmin(this IPrincipal principal)
        {
            return principal.Identity.IsAuthenticated && principal.IsInRole(AppConst.Auth.Admin);
        }

        public static long GetUserIdLong(this IIdentity identity)
        {
            return !identity.IsAuthenticated ? 0 : identity.GetUserId<long>();
        }
        public static UserType GetUserType(this IIdentity identity)
        {
            try
            {
                if (!identity.IsAuthenticated)
                    return default;

                var claims = (ClaimsIdentity)identity;
                var type = claims.FindFirst("UserType");
                if (type != null)
                    return (UserType)int.Parse(type.Value);
            }
            catch
            {
                //
            }

            return default;
        }
    }
}