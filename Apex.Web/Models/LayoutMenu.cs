using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Web.Models
{
    public class LayoutMenu
    {
        public string Key { get; set; }
        public BaseUrl Action { get; set; }
        public string Text { get; set; }
        public bool Hide { get; set; }
        public bool Execlude { get; set; }
        public string CssClass { get; set; }
        public int DisplayOrder { get; set; }

        public string AddUrl { get; set; }
        public string EditUrl { get; set; }
        public string DeleteUrl { get; set; }


        public bool AccountTypeBuyer { get; set; }
        public bool AccountTypeSeller { get; set; }
        public bool AccountTypeCommon { get; set; }
        

        private List<LayoutMenu> Subs { get; set; }
        public List<LayoutMenu> SubMenus
        {
            get => Subs.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Key).ToList();
            set => Subs = value;
        }
        public LayoutMenu()
        {
            Action = new BaseUrl(new UrlHelper(HttpContext.Current.Request.RequestContext), null, null);
            SubMenus = new List<LayoutMenu>();
        }

        public bool HasUrl(string baseUrl)
        {
            var url = baseUrl.ToLower();
            return Action.Url.ToLower() == url || AddUrl.IsNeu("").ToLower() == url || EditUrl.IsNeu("").ToLower() == url || DeleteUrl.IsNeu("").ToLower() == url;
        }
    }

    public class BaseUrl
    {
        private readonly object _routeValues;
        private readonly UrlHelper _urlHelper;

        public BaseUrl(UrlHelper urlHelper, string action, string controller, object routeValues = null)
        {
            _urlHelper = urlHelper;
            Controller = controller;
            _routeValues = routeValues;
            Action = action;
        }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Url
        {
            get
            {
                try
                {
                    return Controller.IsNeu() || Action.IsNeu() ? AppConst.Ui.JsVoid :
            (_routeValues == null ? _urlHelper.Action(Action, Controller) : _urlHelper.Action(Action, Controller, _routeValues));
                }
                catch (Exception)
                {
                    return AppConst.Ui.JsVoid;
                }
            }
        }
    }
}