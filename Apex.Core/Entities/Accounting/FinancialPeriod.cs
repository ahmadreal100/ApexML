using System;
using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    //دوره مالی
    public class FinancialPeriod : Entity
    {
        public bool IsActive { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
    }
}
