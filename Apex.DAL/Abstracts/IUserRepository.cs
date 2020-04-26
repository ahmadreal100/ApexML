using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Apex.Core.Entities.UserE;
using Microsoft.AspNet.Identity;

namespace Apex.DAL.Abstracts
{
    public interface IUserRepository : IRepository<User>
    {
        IdentityResult CreateUser(User user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(User user, string type);
        void CreateRole(string name);
        bool RoleExist(string name);
        void AddToRole(string username, string roleName);
        void AddToRoleById(long userId, string roleName);
        bool IsInRole(long userId, string roleName);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        IdentityResult DeleteUser(User user);
        Task<IdentityResult> DeleteUserAsync(User user);
        bool UserExist(string userName);
        Task<User> FindByNameAsync(string userName);
        User FindByName(string userName);
        Task<User> FindByMailAsync(string email);
        Task<User> FindByIdAsync(long id);
        Task<User> FindAsync(string username, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        void UpdatePermissions(long userId, IEnumerable<MenuPermission> permissions);
    }
}
