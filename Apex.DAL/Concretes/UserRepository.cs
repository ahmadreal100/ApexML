using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Abstract;
using Apex.Core.Entities.UserE;
using Apex.DAL.Abstracts;
using Apex.DAL.EF;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Apex.DAL.Concretes
{
    public class UserRepository : Repository<User>, IUserRepository
    {

        #region Private Fields
        private readonly UserManager<User, long> _userManager;
        private readonly RoleManager<Role, long> _roleManager;
        #endregion Private Fields

        #region Protected Fields

        #endregion Protected Fields


        public UserRepository(ApexContext context, IUnitOfWork unitOfWork, RequestInfo info)
            : base(context, unitOfWork, info)
        {
            _userManager = new UserManager<User, long>(new UserStore<User, Role, long, UserLogin, UserRole, UserClaim>(context));
            _roleManager = new RoleManager<Role, long>(new RoleStore<Role, long, UserRole>(context));
        }

        public IdentityResult CreateUser(User user, string password)
        {
            var result = _userManager.Create(user, password);
            user.MasterInfo = new MasterInfo();
            return result;
        }


        public Task<ClaimsIdentity> CreateIdentityAsync(User user, string type)
        {
            return _userManager.CreateIdentityAsync(user, type);
        }

        public void CreateRole(string name)
        {
            _roleManager.Create(new Role { Name = name });
        }

        public bool RoleExist(string name)
        {
            return _roleManager.RoleExists(name);
        }

        public void AddToRole(string username, string roleName)
        {
            var user = _userManager.FindByName(username);
            if (user == null)
                return;
            _userManager.AddToRole(user.Id, roleName);
        }

        public void AddToRoleById(long userId, string roleName)
        {
            _userManager.AddToRole(userId, roleName);
        }

        public bool IsInRole(long userId, string roleName)
        {
            return _userManager.IsInRole(userId, roleName);
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user);
        }

        public IdentityResult DeleteUser(User user)
        {
            return _userManager.Delete(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public bool UserExist(string userName)
        {
            return _userManager.FindByName(userName) != null;
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public User FindByName(string userName)
        {
            return _userManager.FindByName(userName);
        }

        public async Task<User> FindByMailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> FindByIdAsync(long id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User> FindAsync(string username, string password)
        {
            return await _userManager.FindAsync(username, password);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public void UpdatePermissions(long userId, IEnumerable<MenuPermission> permissions)
        {
            Context.MenuPermissions.RemoveRange(Context.MenuPermissions.Where(m => m.OperatorInfoUserId == userId));

            foreach (var item in permissions)
            {
                Context.MenuPermissions.Add(new MenuPermission
                {
                    MenuId = item.MenuId,
                    OperatorInfoUserId = userId,
                    Add = item.Add,
                    Edit = item.Edit,
                    Delete = item.Delete
                });
            }
        }
    }
}