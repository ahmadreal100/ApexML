namespace Apex.Service.ViewModels.Account
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}