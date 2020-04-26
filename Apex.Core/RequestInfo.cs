using Apex.Core.Enums;

namespace Apex.Core
{
    public class RequestInfo
    {
        public long UserId { get; set; }
        public long LangId { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }
        public bool IsAdmin => UserType == UserType.Admin;
        public bool IsOperator => UserType == UserType.Operator;
    }
}

