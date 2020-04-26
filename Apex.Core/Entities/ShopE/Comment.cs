using Apex.Core.Abstract;
using Apex.Core.Entities.UserE;

namespace Apex.Core.Entities.ShopE
{
    public class Comment : Entity
    {

        public bool Seen { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long RelatedId { get; set; }
        public bool IsApproved { get; set; }
        public string Content { get; set; }
    }
}