using Newtonsoft.Json;

namespace Apex.Core.Helpers
{
    public static class ObjectExtension
    {
        public static string Stringify(this object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static TModel Objectify<TModel>(this string model)
        {
            try
            {
                return JsonConvert.DeserializeObject<TModel>(model);
            }
            catch
            {
                return default;
            }
        }
    }
}
