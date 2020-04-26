using Apex.Core.Entities.UserE;

namespace Apex.Service.Extensions
{
    public static class UserExtension
    {
        public static string FullName(this User user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
    }
}