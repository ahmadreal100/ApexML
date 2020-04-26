using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using Apex.Shared.Extensions;

namespace Apex.Web.Helpers
{
    public static class BundleHelper
    {
        private const string Url = "~/";

        #region Bundle

        public static readonly BundleLayout Layout = new BundleLayout
        {
            LayoutStyleBefore = new List<string>
                {
                    $"{Url}assets/global/plugins/font-awesome/css/font-awesome.min.css",
                    $"{Url}assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                    $"{Url}assets/global/plugins/bootstrap/css/bootstrap-rtl.min.css",
                    $"{Url}assets/global/plugins/uniform/css/uniform.default.css"
                },
            LayoutStyleAfter = new List<string>
                {
                    $"{Url}assets/global/css/components-rounded-rtl.css",
                    $"{Url}assets/global/css/plugins-rtl.css",
                    $"{Url}assets/admin/layout4/css/layout-rtl.css",
                    $"{Url}assets/admin/layout4/css/themes/default-rtl.css",
                    $"{Url}assets/admin/layout4/css/custom-rtl.css",
                    $"{Url}Content/jquery.magnify.css",
                    $"{Url}Content/site.css"
                },
            LayoutScriptBefore = new List<string>
                {
                    $"{Url}assets/global/plugins/jquery.min.js",
                    $"{Url}assets/global/plugins/jquery-migrate.min.js",
                    $"{Url}assets/global/plugins/jquery-ui/jquery-ui.min.js",
                    $"{Url}assets/global/plugins/bootstrap/js/bootstrap.min.js",
                    $"{Url}assets/global/plugins/jquery.blockui.min.js",
                    $"{Url}assets/global/plugins/uniform/jquery.uniform.min.js",
                    $"{Url}assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                    $"{Url}Scripts/jquery.magnify.min.js",
                    $"{Url}Scripts/jquery.signalR-2.2.2.min.js",
                    $"{Url}signalr/hubs",
                    $"{Url}Scripts/arhelper.js"
                },
            LayoutScriptAfter = new List<string>
                {
                    $"{Url}Scripts/ar.custom.validate.js",
                    $"{Url}assets/global/scripts/metronic.js",
                    $"{Url}assets/admin/layout4/scripts/layout.js",
                    $"{Url}assets/admin/layout4/scripts/demo.js",
                    $"{Url}Scripts/global.actions.js",
                    $"{Url}Scripts/searchmenu.js",
                    $"{Url}Scripts/site.js"
                },
            Bodies = new List<BundleBody>
                {
                    new BundleBody
                    {
                        Controller = "Home",
                        Action = "Index",
                        Scripts = new List<string>
                        {
                            $"{Url}assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                            $"{Url}Scripts/dashboard.js"
                        }
                    }
                }
        };

        #endregion

        public static List<Bundle> GetAll => Get();

        private static List<Bundle> Get()
        {
            var scriptBundles = new List<Bundle>();
            var styleBundles = new List<Bundle>();
            Layout.Bodies.ForEach(bb =>
            {
                var scriptConcat = Layout.LayoutScriptBefore.Concat(bb.Scripts).Concat(Layout.LayoutScriptAfter);
                scriptBundles.Add(new ScriptBundle(bb.ScriptName).Include(scriptConcat.ToArray()));

                var styleConcat = Layout.LayoutStyleBefore.Concat(bb.Styles).Concat(Layout.LayoutStyleAfter);
                styleBundles.Add(new StyleBundle(bb.StyleName).Include(styleConcat.ToArray()));
            });

            return scriptBundles.Concat(styleBundles).ToList();
        }

        public static void Select(string controller, string action, out List<Bundle> scripts, out List<Bundle> styles)
        {
            scripts = new List<Bundle>();
            styles = new List<Bundle>();
            var bodyBundle = Layout.Bodies.FirstOrDefault(x =>
                string.Equals(x.Controller, controller, StringComparison.CurrentCultureIgnoreCase) &&
                string.Equals(x.Action, action, StringComparison.CurrentCultureIgnoreCase));
            if (bodyBundle == null) return;

            var scriptConcat = Layout.LayoutScriptBefore.Concat(bodyBundle.Scripts).Concat(Layout.LayoutScriptAfter);
            scripts.Add(new ScriptBundle(bodyBundle.ScriptName).Include(scriptConcat.ToArray()));

            var styleConcat = Layout.LayoutStyleBefore.Concat(bodyBundle.Styles).Concat(Layout.LayoutStyleAfter);
            styles.Add(new StyleBundle(bodyBundle.StyleName).Include(styleConcat.ToArray()));
        }
    }

    #region Classes

    public class BundleLayout
    {
        public BundleLayout()
        {
            LayoutStyleBefore = new List<string>();
            LayoutStyleAfter = new List<string>();
            LayoutScriptBefore = new List<string>();
            LayoutScriptAfter = new List<string>();
        }

        public List<BundleBody> Bodies { get; set; }
        public List<string> LayoutStyleBefore { get; set; }
        public List<string> LayoutStyleAfter { get; set; }
        public List<string> LayoutScriptBefore { get; set; }
        public List<string> LayoutScriptAfter { get; set; }
    }

    public class BundleBody
    {
        public BundleBody()
        {
            Scripts = new List<string>();
            Styles = new List<string>();
        }

        public string Action { get; set; }
        public string Controller { get; set; }
        public List<string> Scripts { get; set; }
        public List<string> Styles { get; set; }
        public string ScriptName => $"~/Scripts/{$"{Controller.ToLower()}-{Action.ToLower()}".ToHashedGuid()}";
        public string StyleName => $"~/Content/{$"{Controller.ToLower()}-{Action.ToLower()}".ToHashedGuid()}";

        public static BundleBody Create(int themeId, string controller, string action)
        {
            return new BundleBody { Controller = controller, Action = action };
        }
    }

    #endregion
}