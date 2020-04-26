using System.Collections.Generic;
using Accounting.Core.Entities.DetailedE;
using Accounting.Core.Enums;
using Newtonsoft.Json;

namespace Accounting.Core.Entities.Accounting
{
    //حساب معین
    public class CodingSubsidiary : CodeBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// تعداد سطوح تفصیل
        /// </summary>
        public int SubDetailMaxLevel { get; set; }
        //public string LedgerCode { get; set; }
        public long LedgerId { get; set; }
        public CodingGroupNature Nature { get; set; }

        [JsonIgnore]
        public virtual CodingLedger Ledger { get; set; }
        [JsonIgnore]
        public ICollection<SubsidiaryCategoryPin> Categories { get; set; }
    }
}