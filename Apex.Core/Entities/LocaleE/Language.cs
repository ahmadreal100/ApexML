using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Abstract;

namespace Apex.Core.Entities.LocaleE
{
    public class Language : Entity
    {
        public bool Rtl { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(5)]
        public string Locale { get; set; }
        public bool IsDefault { get; set; }
        public string PicLink { get; set; }
    }
}