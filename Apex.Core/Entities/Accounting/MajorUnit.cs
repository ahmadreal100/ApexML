using System.Collections.Generic;
using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class MajorUnit : Entity
    {
        public string Title { get; set; }
        public ICollection<Goods> Goodses { get; set; }
    }
}