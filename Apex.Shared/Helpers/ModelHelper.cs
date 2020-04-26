using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Apex.Shared.Helpers
{
    public static class ModelHelper
    {
        public static void SetPersianDate(object obj)
        {
            SetPDate(ref obj);
            void SetPDate(ref object o)
            {
                var type = o.GetType();
                var props = type.GetProperties();
                var names = props.ToList().Select(x => x.Name).ToList();
                foreach (var info in props)
                {
                    if (info.Name.EndsWith("P"))
                    {
                        var gregName = Regex.Replace(info.Name, "P\\b", "");
                        if (names.Contains(gregName))
                        {
                            var gregVal = (DateTime?)type.GetProperty(gregName)?.GetValue(o);
                            if (gregVal != null) info.SetValue(o, gregVal.Value.ToPersian(), null);
                        }
                    }
                    var itype = info.GetType();
                    if (itype.Module.ScopeName != "CommonLanguageRuntimeLibrary")
                        itype.GetProperties().ToList().ForEach(x =>
                        {
                            var hasDot = type.GetProperty(info.Name);
                            var hasDotVal = hasDot?.GetValue(info);
                            if (hasDot != null)
                                SetPDate(ref hasDotVal);
                        });
                }
            }
        }
    }
}
