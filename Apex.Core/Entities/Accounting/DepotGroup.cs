using System.Collections.Generic;
using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    //گروه بندی انبار
    public class DepotGroup : Entity
    {
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public DepotGroup Parent { get; set; }
        public ICollection<DepotGroup> Childs { get; set; }
    }
}