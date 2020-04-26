
namespace Accounting.Core.Entities.Accounting
{
    //انبار
    public class Depot : CodeBase
    {
        public string Title { get; set; }
        public string LiableName { get; set; }
        public long GroupId { get; set; }
        public DepotGroup Group { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}