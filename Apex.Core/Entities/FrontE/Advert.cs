using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apex.Core.Abstract;
using Apex.Core.Entities.LocaleE;

namespace Apex.Core.Entities.FrontE
{
    public class Advert : Entity
    {
        public int Location { get; set; }
        public string PicLink { get; set; }
        public int DisplayOrder { get; set; }

        //Translation
        public ICollection<AdvertTranslation> Translations { get; set; }
    }
    //Translation
    public class AdvertTranslation : Translation
    {
        [Key, Column(Order = 2)]
        public long AdvertId { get; set; }
        public Advert Advert { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}