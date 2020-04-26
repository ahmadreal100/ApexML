using Apex.Core.Abstract;
using Apex.Core.Entities.LocaleE;
using Apex.Core.Entities.ShopE;

namespace Apex.Core.Entities.FrontE
{
    public class Tag : Entity
    {
        public long LanguageId { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public Language Language { get; set; }
        public Product Product { get; set; }
    }
}