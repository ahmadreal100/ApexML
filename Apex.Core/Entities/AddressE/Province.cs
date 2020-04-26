using System.Collections.Generic;
using Apex.Core.Abstract;

namespace Apex.Core.Entities.AddressE
{
    public class Province : Entity
    {
        public string Name { get; set; }
        public long CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}