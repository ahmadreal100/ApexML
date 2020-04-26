using System.Collections.Generic;
using Accounting.Core.Enums;
using Newtonsoft.Json;

namespace Accounting.Core.Entities.Accounting
{
    //حساب کل
    public class CodingLedger : CodeBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        //public string GroupCode { get; set; }
        public long GroupId { get; set; }
        [JsonIgnore]
        public virtual CodingGroup Group { get; set; }
        public ICollection<CodingSubsidiary> Subsidiaries { get; set; }

    }
}