using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Accounting.Core.Enums;

namespace Accounting.Core.Entities.Accounting
{
    //گروه حساب
    public class CodingGroup : CodeBase
    {
        public CodingSubNature SubNature { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CodingGroupNature GroupNature { get; set; }
        public ICollection<CodingLedger> Ledgers { get; set; }
    }
}