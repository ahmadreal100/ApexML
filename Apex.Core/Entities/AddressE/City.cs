using Apex.Core.Abstract;

namespace Apex.Core.Entities.AddressE
{
    public class City : Entity
    {
        public string Name { get; set; }
        public long ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}