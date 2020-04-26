using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class PersonGroup : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Seller { get; set; }
        public bool Buyer { get; set; }
    }
}
