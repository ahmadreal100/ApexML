using Apex.Web.Enums;

namespace Apex.Web.Models
{
    public class TempResult
    {
        public string Key { get; set; }
        public MessageType Type { get; set; }
        public string Message { get; set; }
    }
}