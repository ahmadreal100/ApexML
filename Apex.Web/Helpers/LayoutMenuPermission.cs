using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using Apex.Web.Models;

namespace Apex.Web.Helpers
{
    public static class OptPermission
    {
        private static bool IsMatch { get; set; }
        private static List<LayoutMenu> OutMenus { get; set; }
        private static LayoutMenu OutMenu { get; set; }
        public static List<LayoutMenu> LayoutMenu(UrlHelper urlHelper)
        {
            return new List<LayoutMenu>
            {
                new LayoutMenu
                {
                    Key = "100",
                    Text = "صفحه اصلی",
                    CssClass = "fa-home",
                    Action = new BaseUrl(urlHelper, "Index", "Home"),
                    Execlude = true,
                    AccountTypeBuyer = true,
                    AccountTypeSeller = true,
                    AccountTypeCommon = true

                },
                new LayoutMenu
                {
                    Key = "101",
                    Text = "سیستم",
                    CssClass = "fa-star-o",
                    SubMenus = new List<LayoutMenu>
                    {
                        new LayoutMenu
                        {
                            Key = "101001",
                            Text = "مدیریت کاربران",
                            CssClass = "fa-user-secret",
                            Action = new BaseUrl(urlHelper, "Index", "Operator"),
                            AddUrl = new BaseUrl(urlHelper, "Add", "Operator").Url,
                            EditUrl = new BaseUrl(urlHelper, "Update", "Operator").Url,
                            DeleteUrl = new BaseUrl(urlHelper, "Delete", "Operator").Url,
                            SubMenus = new List<LayoutMenu>
                            {
                                new LayoutMenu
                                {
                                    Key = "101001000",
                                    Text = "سطح دسترسی",
                                    Hide = true,
                                    EditUrl = new BaseUrl(urlHelper, "SetPermissions", "Operator").Url
                                },
                                new LayoutMenu
                                {
                                    Key = "101001002",
                                    Text = "تغییر رمز",
                                    Hide = true,
                                    EditUrl = new BaseUrl(urlHelper, "ChangePassword", "Operator").Url
                                }
                            }
                        },
                        new LayoutMenu
                        {
                            Key = "101002",
                            Text = "تنظیمات",
                            CssClass = "fa-gears",
                            Action = new BaseUrl(urlHelper, "Index", "Setting"),
                            SubMenus = new List<LayoutMenu>
                            {
                                new LayoutMenu
                                {
                                    Key = "101002000",
                                    Text = "قالب",
                                    Hide = true,
                                    EditUrl = new BaseUrl(urlHelper, "UpdateTheme", "Setting").Url
                                }
                            }
                        }
                    }
                },
                new LayoutMenu
                {
                    Key = "102",
                    Text = "تعاریف",
                    CssClass = "fa-bank",
                    SubMenus = new List<LayoutMenu>
                    {
                        new LayoutMenu
                        {
                            Key = "102000",
                            Text = "دسته بندی",
                            CssClass = "fa-list-ul",
                            Action = new BaseUrl(urlHelper, "Index", "Category"),
                            AddUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Category").Url,
                            EditUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Category").Url,
                            DeleteUrl = new BaseUrl(urlHelper, "Delete", "Category").Url
                        },
                        new LayoutMenu
                        {
                            Key = "102001",
                            Text = "مطلب",
                            CssClass = "fa-cube",
                            Action = new BaseUrl(urlHelper, "Index", "Product"),
                            AddUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Product").Url,
                            EditUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Product").Url,
                            DeleteUrl = new BaseUrl(urlHelper, "Delete", "Product").Url
                        }
                    }
                },
                new LayoutMenu
                {
                    Key = "103",
                    Text = "مدیریت محتوا",
                    CssClass = "fa-leaf",
                    SubMenus = new List<LayoutMenu>
                    {
                        new LayoutMenu
                        {
                            Key = "103000",
                            Text = "اسلایدر",
                            CssClass = "fa-television",
                            Action = new BaseUrl(urlHelper, "Index", "Slider"),
                            AddUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Slider").Url,
                            EditUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Slider").Url,
                            DeleteUrl = new BaseUrl(urlHelper, "Delete", "Slider").Url
                        }
                    }
                },
                new LayoutMenu
                {
                    Key = "104",
                    Text = "بازخورد",
                    CssClass = "fa-flash",
                    SubMenus = new List<LayoutMenu>
                    {
                        //new LayoutMenu
                        //{
                        //    Key = "104000",
                        //    Text = "نظرات مطلب",
                        //    CssClass = "fa-comment-o",
                        //    Action = new BaseUrl(urlHelper, "Index", "ProductComment"),
                        //    //AddUrl = new BaseUrl(urlHelper, "AddOrUpdate", "ProductComment").Url,
                        //    EditUrl = new BaseUrl(urlHelper, "AddOrUpdate", "ProductComment").Url,
                        //    DeleteUrl = new BaseUrl(urlHelper, "Delete", "ProductComment").Url
                        //},
                        new LayoutMenu
                        {
                            Key = "104001",
                            Text = "نظرات وبسایت",
                            CssClass = "fa-comments-o",
                            Action = new BaseUrl(urlHelper, "Index", "Comment"),
                            //AddUrl = new BaseUrl(urlHelper, "AddOrUpdate", "Comment").Url,
                            EditUrl = new BaseUrl(urlHelper, "Seen", "Comment").Url,
                            DeleteUrl = new BaseUrl(urlHelper, "Delete", "Comment").Url
                        }
                    }
                }
            };
        }
        public static string AttrName<T>(Expression<Func<T, string>> expression)
        {
            var property = typeof(T).GetProperty((expression.Body as MemberExpression)?.Member.Name ?? throw new InvalidOperationException());
            return property?.GetCustomAttribute<DisplayAttribute>()?.Name;
        }

        public static bool AnyNested(this IEnumerable<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            IsMatch = false;
            var items = menus.ToList();
            if (items.Any(func))
                return true;

            DelegateAny(items, func);
            return IsMatch;
        }

        private static void DelegateAny(List<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            foreach (var menu in menus)
            {
                if (menu.SubMenus.Any(func))
                {
                    IsMatch = true;
                    break;
                }

                if (menu.SubMenus.Any())
                    DelegateAny(menu.SubMenus, func);
            }
        }

        public static List<LayoutMenu> WhereNested(this IEnumerable<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            OutMenus = menus.Where(func).ToList();
            DelegateWhere(OutMenus, func);
            return OutMenus;
        }

        private static void DelegateWhere(List<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            menus.ForEach(menu =>
               {
                   menu.SubMenus = menu.SubMenus.Where(func).ToList();
                   if (menu.SubMenus.Any())
                       DelegateWhere(menu.SubMenus, func);
               });
        }

        public static LayoutMenu FirstNested(this IEnumerable<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            var mLi = menus.ToList();
            OutMenu = mLi.FirstOrDefault(func);
            if (OutMenu != null)
                return OutMenu;
            DelegateFirst(mLi, func);
            return OutMenu;
        }

        private static void DelegateFirst(List<LayoutMenu> menus, Func<LayoutMenu, bool> func)
        {
            menus.ForEach(menu =>
               {
                   var m = menu.SubMenus.FirstOrDefault(func);
                   if (m != null)
                   {
                       OutMenu = m;
                       return;
                   }
                   if (menu.SubMenus.Any())
                       DelegateFirst(menu.SubMenus, func);
               });
        }
    }
}