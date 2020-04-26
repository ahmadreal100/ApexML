using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Entities.AddressE;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Enums;

namespace Apex.Core.Entities.UserE
{
    public class MasterInfo
    {
        public User User { get; set; }
        [Key, ForeignKey("User")]
        public long UserId { get; set; }
        public string LogoLink { get; set; }

        public AccountStatus AccountStatus { get; set; }
        public string PostalCode { get; set; }
        [StringLength(128)]
        public string Mobile { get; set; }
        [StringLength(128)]
        public string Email { get; set; }
        public long? ProvinceId { get; set; }
        public Province Province { get; set; }
        public long? CityId { get; set; }
        public City City { get; set; }
        public ICollection<MasterInfoTranslation> Translations { get; set; }
    }

    public class MasterInfoTranslation : Translation
    {
        [Key, Column(Order = 2)]

        public long MasterInfoUserId { get; set; }
        [ForeignKey("MasterInfoUserId")]
        public MasterInfo MasterInfo { get; set; }

        public string ManagerName { get; set; }
        public string ManagerFamily { get; set; }
        public string BusinessName { get; set; }
        public string CommercialName { get; set; }
        public string Address { get; set; }
    }
}
