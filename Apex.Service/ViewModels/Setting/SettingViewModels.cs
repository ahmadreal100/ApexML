using Apex.Service.ViewModels.Account;

namespace Apex.Service.ViewModels.Setting
{
    public class SettingViewModel
    {
        public MasterInfoViewModel BaseUserInfo { get; set; }
        public ThemeSettingViewModel ThemeSetting { get; set; }
    }
}