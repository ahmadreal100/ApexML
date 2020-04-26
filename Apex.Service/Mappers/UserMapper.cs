using Apex.Core.Entities.UserE;
using Apex.Service.ViewModels.Account;
using AutoMapper;

namespace Apex.Service.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper() : this("user")
        {
            CreateMap<User, UserViewModel>();

            CreateMap<UserViewModel, User>()
                .ForMember(x => x.Id, y => y.Ignore());

            CreateMap<MenuPermissionViewModel, MenuPermission>();
            CreateMap<MenuPermission, MenuPermissionViewModel>();

            CreateMap<OperatorInfoViewModel, OperatorInfo>();
            CreateMap<OperatorInfo, OperatorInfoViewModel>();
        }

        protected UserMapper(string profileName) : base(profileName)
        {
        }

    }
}
