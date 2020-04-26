using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class GoodsGroup : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Parent")]
        public long? ParentId { get; set; }
        public GoodsGroup Parent { get; set; }
        public ICollection<GoodsGroup> Childs { get; set; }
        public ICollection<Goods> Goodses { get; set; }
    }
}