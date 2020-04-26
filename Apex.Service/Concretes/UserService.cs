using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Apex.Core;
using Apex.Core.Entities.AddressE;
using Apex.Core.Entities.UserE;
using Apex.DAL.Abstracts;
using Apex.Service.Abstracts;
using Apex.Service.ViewModels.Account;
using Apex.Service.ViewModels.Setting;
using Apex.Shared.Helpers;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Enums;
using Apex.Core.Helpers;

namespace Apex.Service.Concretes
{
    public class UserService : Service<User>, IUserService
    {
        public IUserRepository UserRepository { get; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Country> _countryRepository;
        private readonly ILanguageRepository _languageRepository;
        //private IValidator<User> _validator;
        public UserService(RequestInfo info, IUserRepository repository,
            IUserRepository userRepository, IUnitOfWork unitOfWork,
            IRepository<Country> countryRepository, ILanguageRepository languageRepository) :
            base(info, repository, unitOfWork)
        {
            UserRepository = userRepository;
            _unitOfWork = unitOfWork;
            _countryRepository = countryRepository;
            _languageRepository = languageRepository;
        }

        public RequestInfo RequestInfo => Info;
        public User FindByUserName(string userName)
        {
            return UserRepository.FindByName(userName);
        }

        public IdentityResult CreateUser(User user)
        {
            return UserRepository.CreateUser(user, user.PasswordHash);
        }

        public async Task<ServiceResult<UserViewModel>> Create(UserViewModel model)
        {
            try
            {
                var res = new ServiceResult<UserViewModel>();

                var user = Mapper.Map<User>(model);
                user.PasswordHash = new PasswordHasher().HashPassword(model.Password);


                var ir = await UserRepository.CreateUserAsync(user, model.Password);
                ir.Errors.ToList().ForEach(m => res.State.Errors.Add(m, m, m));

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ServiceResult<UserViewModel>> Update(long id, UserViewModel model)
        {
            var res = new ServiceResult<UserViewModel>();

            var user = UserRepository.OneAsset(id, m => m.OperatorInfo);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            Mapper.Map(model, user);

            try
            {
                UserRepository.Update(user);
                await UnitOfWork.SaveChangesAsync();
            }
            catch
            {
                res.ServerError();
                UnitOfWork.Rollback();
            }
            return res;
        }
        public async Task<ServiceResult<UserViewModel>> Delete(long id)
        {
            var res = new ServiceResult<UserViewModel>();

            var user = UserRepository.OneAsset(id);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            try
            {
                await UserRepository.DeleteUserAsync(user);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            return res;
        }

        #region Operator

        public async Task<ServiceResult<UserViewModel>> SetOperatorPermission(long userId, List<MenuPermissionViewModel> model)
        {
            var res = new ServiceResult<UserViewModel>();
            var user = UserRepository.Asset(userId).Include(x => x.OperatorInfo.MenuPermissions).FirstOrDefault(x => x.Type == UserType.Operator);

            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            if (user.OperatorInfo == null)
                user.OperatorInfo = new OperatorInfo();

            var permissions = Mapper.Map<List<MenuPermission>>(model);
            UserRepository.UpdatePermissions(userId, permissions);

            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }
            return res;
        }

        public List<MenuPermissionViewModel> GetOperatorPermissions(long userId)
        {
            var user = UserRepository.OneAsset(userId, x => x.OperatorInfo.MenuPermissions);
            if (user?.OperatorInfo == null)
                return new List<MenuPermissionViewModel>();

            var permissions = Mapper.Map<List<MenuPermissionViewModel>>(user.OperatorInfo.MenuPermissions);
            return permissions;
        }

        #endregion

        #region Setting

        public async Task<ServiceResult<ThemeSettingViewModel>> SetThemeSetting(ThemeSettingViewModel model)
        {
            var res = new ServiceResult<ThemeSettingViewModel>();

            var user = UserRepository.Queryable().Where(x => x.Id == RequestInfo.UserId).Include(x => x.ThemeSetting).FirstOrDefault();
            if (user == null)
            {
                res.State.Errors.Add("", "", "");
                return res;
            }

            if (user.ThemeSetting == null)
                user.ThemeSetting = new ThemeSetting { Translations = new List<ThemeSettingTranslation>() };

            Mapper.Map(model, user.ThemeSetting);
            _languageRepository.RemoveThemeSettingFromLanguage(RequestInfo.UserId);
            Repository.Update(user);

            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            res.Result = Mapper.Map<ThemeSettingViewModel>(user.ThemeSetting);
            return res;
        }

        public ThemeSettingViewModel GetThemeSetting()
        {
            var user = UserRepository.Queryable().Where(x => x.Type == UserType.Admin)
                .Include(x => x.ThemeSetting.Translations).FirstOrDefault();

            return Mapper.Map<ThemeSettingViewModel>(user?.ThemeSetting ?? new ThemeSetting());
        }

        public async Task<ThemeSettingViewModel> GetThemeSettingAsync()
        {
            var user = await UserRepository.Queryable().Where(x => x.Type == UserType.Admin)
                .Include(x => x.ThemeSetting.Translations).FirstOrDefaultAsync();

            return Mapper.Map<ThemeSettingViewModel>(user?.ThemeSetting ?? new ThemeSetting());
        }

        #endregion

        public void Seed(string locs, string langPath)
        {
            if (!_countryRepository.Queryable().Any())
                SeedLocations(locs);

            if (!UserRepository.RoleExist(AppConst.Auth.Admin))
                UserRepository.CreateRole(AppConst.Auth.Admin);

            var admin = UserRepository.FindByName(AppConst.Auth.Admin);
            if (admin == null)
            {
                admin = new User { UserName = AppConst.Auth.Admin, Type = UserType.Admin };
                UserRepository.CreateUser(admin, AppConst.Auth.DevPass);
                UserRepository.AddToRoleById(admin.Id, AppConst.Auth.Admin);
            }

            var json = File.ReadAllText(langPath);
            var languages = json.Objectify<List<Language>>();
            foreach (var language in languages)
            {
                if (!_languageRepository.Queryable().Any(x => x.Locale.ToLower() == language.Locale.ToLower()))
                    _languageRepository.Insert(language);
            }


            _unitOfWork.SaveChanges();
        }


        public void SeedLocations(string path)
        {
            using (var r = new StreamReader(path))
            {
                var json = r.ReadToEnd();
                var country = JsonConvert.DeserializeObject<Country>(json);

                _countryRepository.Insert(country);
            }

            _unitOfWork.SaveChanges();
        }

        public bool IsInRole(long userId, string roleName)
        {
            return UserRepository.IsInRole(userId, roleName);
        }

        public async Task<ServiceResult<MasterInfoViewModel>> Update(long id, MasterInfoViewModel model)
        {
            var res = new ServiceResult<MasterInfoViewModel>();

            var user = Repository.Find(id);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            if (user.MasterInfo == null)
                user.MasterInfo = new MasterInfo();

            Mapper.Map(model, user);
            Mapper.Map(model, user.MasterInfo);

            //user.MasterInfo.Translations.Add(Mapper.Map<MasterInfoTranslation>(model));
            _languageRepository.RemoveMasterInfoFromLanguage(user.Id);
            Repository.Update(user);
            res.Result = MasterInfoViewModel.Map(user);
            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }
            return res;
        }
        public async Task<ServiceResult<RegisterViewModel>> UpdateUserInfo(RegisterViewModel model)
        {
            var res = new ServiceResult<RegisterViewModel>();
            var user = Repository.Find(model.Id);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            user.FirstName = model.FirstName;

            UserRepository.Update(user);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }
            return res;
        }

        public async Task<ServiceResult<ChangePasswordViewModel>> ChangeUserPassword(long userId, ChangePasswordViewModel model)
        {
            var res = new ServiceResult<ChangePasswordViewModel> { Result = model };
            var user = UserRepository.Find(userId);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            if (!await UserRepository.CheckPasswordAsync(user, model.CurrentPassword))
            {
                res.State.Errors.Add("6000", model, "current pass is wrong");
                return res;
            }

            user.PasswordHash = new PasswordHasher().HashPassword(model.NewPassword);

            try
            {
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                res.ServerError();
            }

            return res;
        }

        public async Task<ServiceResult<ResetPasswordViewModel>> ResetPassword(ResetPasswordViewModel model)
        {
            var res = new ServiceResult<ResetPasswordViewModel> { Result = model };
            var user = UserRepository.Queryable().FirstOrDefault(x => x.Id == Info.UserId);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            if (!await UserRepository.CheckPasswordAsync(user, model.CurrnetPassword))
            {
                res.Result.WrongHashedPassword = true;
                return res;
            }

            user.PasswordHash = new PasswordHasher().HashPassword(model.NewPassword);
            await UnitOfWork.SaveChangesAsync();
            return res;
        }


        public async Task<ServiceResult<UserViewModel>> ChangePassword(long id, string password)
        {
            var res = new ServiceResult<UserViewModel>();
            var user = UserRepository.OneAsset(id);
            if (user == null)
            {
                res.NotFound = true;
                return res;
            }

            user.PasswordHash = new PasswordHasher().HashPassword(password);

            await UnitOfWork.SaveChangesAsync();
            res.Result = Mapper.Map<UserViewModel>(user);
            return res;
        }

        public async Task<bool> UserNameExist(string userName, long id)
        {
            return await UserRepository.Queryable().AnyAsync(x => x.UserName == userName && x.Id != id);
        }
    }
}
