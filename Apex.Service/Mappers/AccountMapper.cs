using Apex.Core.Entities.UserE;
using Apex.Service.ViewModels.Account;
using AutoMapper;

namespace Apex.Service.Mappers
{
    public class AccountMapper : Profile
    {
        public AccountMapper() : this("account")
        {
            CreateMap<RegisterViewModel, User>();
            CreateMap<User, RegisterViewModel>();
        }

        protected AccountMapper(string profileName) : base(profileName)
        {
        }

    }
}
