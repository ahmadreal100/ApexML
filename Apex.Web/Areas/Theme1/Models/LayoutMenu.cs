//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Foundation.Shared.Extensions;
//using Foundation.Shared.Helpers;

//namespace Foundation.Web.Models
//{
//    public class LayoutMenu
//    {
//        private List<LayoutMenu> Subs { get; set; }
//        public string Key { get; set; }
//        public BaseUrl Action { get; set; }
//        public string Text { get; set; }
//        public bool Hide { get; set; }
//        public bool Execlude { get; set; }
//        public string CssClass { get; set; }
//        public int DisplayOrder { get; set; }
//        public List<LayoutMenu> SubMenus
//        {
//            get => Subs.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Key).ToList();
//            set => Subs = value;
//        }
//        public LayoutMenu()
//        {
//            Action = new BaseUrl(new UrlHelper(HttpContext.Current.Request.RequestContext), null, null);
//            SubMenus = new List<LayoutMenu>();
//        }
//    }

//    public class BaseUrl
//    {
//        private readonly object _routeValues;
//        private readonly UrlHelper _urlHelper;

//        public BaseUrl(UrlHelper urlHelper, string action, string controller, object routeValues = null)
//        {
//            _urlHelper = urlHelper;
//            Controller = controller;
//            _routeValues = routeValues;
//            Action = action;
//        }
//        public string Controller { get; set; }
//        public string Action { get; set; }
//        public string Url => Controller.IsNeu() || Action.IsNeu() ? AppConst.JsVoid : _urlHelper.Action(Action, Controller, _routeValues);
//    }
//}