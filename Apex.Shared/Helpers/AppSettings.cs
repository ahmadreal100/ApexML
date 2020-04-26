using System;

namespace Apex.Shared.Helpers
{
    public static class AppSettingHelper
    {
        public static T Get<T>(string key)
        {
            var value = new System.Configuration.AppSettingsReader().GetValue(key, typeof(T));
            if (value == null)
                throw new Exception($"Could not find setting '{key}'");
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static string Get(string key) => Get<string>(key);

    }
}