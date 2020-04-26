using System.Collections.Generic;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Entities.UserE;
using Apex.DAL.Abstracts;
using Apex.Service.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Apex.Service.ViewModels.Setting;
using RegisterViewModel = Apex.Service.ViewModels.Account.RegisterViewModel;

namespace Apex.Service.Abstracts
{
    public interface IUserService : IService<User>
    {
        RequestInfo RequestInfo { get; }
        User FindByUserName(string userName);
        IdentityResult CreateUser(User user);
        IUserRepository UserRepository { get; }
        void Seed(string locs,string langPath);
        bool IsInRole(long userId, string roleName);
        Task<ServiceResult<UserViewModel>> Create(UserViewModel model);
        Task<ServiceResult<UserViewModel>> Update(long id, UserViewModel model);
        Task<ServiceResult<UserViewModel>> Delete(long id);
        Task<ServiceResult<MasterInfoViewModel>> Update(long id, MasterInfoViewModel model);
        Task<ServiceResult<UserViewModel>> ChangePassword(long id, string password);
        Task<ServiceResult<ResetPasswordViewModel>> ResetPassword(ResetPasswordViewModel model);
        List<MenuPermissionViewModel> GetOperatorPermissions(long userId);
        ThemeSettingViewModel GetThemeSetting();
        Task<ThemeSettingViewModel> GetThemeSettingAsync();
        Task<ServiceResult<ThemeSettingViewModel>> SetThemeSetting(ThemeSettingViewModel model);
        Task<bool> UserNameExist(string userName, long id);
        Task<ServiceResult<RegisterViewModel>> UpdateUserInfo(RegisterViewModel model);
        Task<ServiceResult<ChangePasswordViewModel>> ChangeUserPassword(long userId, ChangePasswordViewModel model);
    }
}
