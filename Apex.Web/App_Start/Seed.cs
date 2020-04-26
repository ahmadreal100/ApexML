using Apex.Service.Abstracts;

namespace Apex.Web
{
    public static class Seed
    {
        public static void Run(IUserService ac, string locPath, string langPath)
        {
            ac.Seed(locPath,langPath);
        }
    }
}
