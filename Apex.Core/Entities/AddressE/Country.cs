using System.Collections.Generic;
using Apex.Core.Abstract;

namespace Apex.Core.Entities.AddressE
{
    public class Country : Entity
    {
        public string Name { get; set; }
        public ICollection<Province> Provinces { get; set; }
    }
}