using System.Collections.Generic;
using Accounting.Core.Abstract;
using Accounting.Core.Entities.AddressE;
using Accounting.Core.Enums;

namespace Accounting.Core.Entities.Accounting
{
    public class Person : Entity
    {
        public Person()
        {
            Groups = new List<PersonGroupPin>();
        }
        public string Code { get; set; }
        public PersonType Type { get; set; }
        public string NationalCode { get; set; }
        public string NationalId { get; set; }
        public string RegisterationCode { get; set; }
        public string EconomicCode { get; set; }
        public decimal CreditLimit { get; set; }
        public int MaxDebtDays { get; set; }
        public string UserName { get; set; }
        public string HashPassword { get; set; }
        public ICollection<PersonGroupPin> Groups { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}