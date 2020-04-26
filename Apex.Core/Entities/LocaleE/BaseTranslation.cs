using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apex.Core.Entities.LocaleE
{
    public class Translation
    {
        [Key, Column(Order = 1)]
        public long LanguageId { get; set; }
        public Language Language { get; set; }
    }
}