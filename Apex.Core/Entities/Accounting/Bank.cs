using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class Bank : Entity
    {
        public string Code { get; set; }
        public string Branch { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
    }
}
