using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class Phone : Entity
    {
        public long PersonId { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public Person Person { get; set; }
    }
}