using System.Security.Claims;
using System.Threading.Tasks;
using Apex.Core.Abstract;
using Apex.Core.Enums;
using Microsoft.AspNet.Identity;

namespace Apex.Core.Entities.UserE
{
    public class User : Identity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType Type { get; set; }
        public AccountStatus Status { get; set; }
        public virtual MasterInfo MasterInfo { get; set; }
        public virtual ThemeSetting ThemeSetting { get; set; }

        public virtual OperatorInfo OperatorInfo { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            return userIdentity;
        }
    }
}