namespace Apex.Service.ViewModels.Shop
{
    public class VisitViewModel
    {
        public long Id { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string WebBrowser { get; set; }
        public bool IsMobile { get; set; }

        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string PathName { get; set; }
        public string Parameters { get; set; }

        public string UrlReferrer { get; set; }
        public string PersianDate { get; set; }
        public long? UserId { get; set; }
    }
}