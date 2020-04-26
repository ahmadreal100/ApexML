using Apex.Core.Abstract;

namespace Apex.Core.Entities.AddressE
{
    public class Contact : Entity
    {
        public string PhoneNumber { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public Province Province { get; set; }
        public City City { get; set; }
        public long? CityId { get; set; }
        public long? ProvinceId { get; set; }
    }
}