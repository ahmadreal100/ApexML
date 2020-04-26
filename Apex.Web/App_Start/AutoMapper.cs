using Apex.Service.Mappers;
using AutoMapper;

namespace Apex.Web
{
    public static class MapperConfig
    {

        public static void Init()
        {
            Mapper.Initialize(m =>
            {
                m.AddProfile(typeof(AccountMapper));
                m.AddProfile(typeof(SettingMapper));
                m.AddProfile(typeof(ProfileMapper));
                m.AddProfile(typeof(ShopMapper));
                m.AddProfile(typeof(ContentMapper));
                m.AddProfile(typeof(SelectMapper));
                m.AddProfile(typeof(UserMapper));
            });
        }
    }
}
