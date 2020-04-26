using Apex.Core.Abstract;

namespace Apex.Core.Entities.FrontE
{
    public class Visit : Entity
    {
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