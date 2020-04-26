using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apex.Core.Entities.UserE
{
    public class OperatorInfo
    {
        public User User { get; set; }
        [Key, ForeignKey("User")]
        public long UserId { get; set; }

        public ICollection<MenuPermission> MenuPermissions { get; set; }
    }
    public class MenuPermission
    {
        [Key, Column(Order = 1)]
        public long OperatorInfoUserId { get; set; }
        [ForeignKey("OperatorInfoUserId")]
        public OperatorInfo OperatorInfo { get; set; }
        [Key, Column(Order = 2)]
        public string MenuId { get; set; }

        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
