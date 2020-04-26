using System;
using System.Linq;
using System.Web.Mvc;

namespace Apex.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AjaxAttribute : ActionFilterAttribute
    {
        
        // Just use name of SetRequestInfo as Action Attribute. By AHMAD.R
    }

    public class AttributeHelper
    {
        public static bool Has<T>(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(T), true).Any() ||
                filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(typeof(T), true).Any())
                return true;
            return false;
        }
        public static bool Has<T>(ActionExecutedContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(T), true).Any() ||
                filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(typeof(T), true).Any())
                return true;
            return false;
        }
    }
}