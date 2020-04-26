using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Apex.Service.ViewModels
{
    public class CodingBaseData
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long? GroupId { get; set; }
    }

    public class PeriodParamsModel
    {
        [Display(Name = "از تاریخ")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "تا تاریخ")]
        public DateTime? EndDate { get; set; }
    }

    public class ReportParamsModel : PeriodParamsModel
    {
        public ReportParamsModel()
        {
            Ids = new List<long>();
            Ids2 = new List<long>();
        }
        public List<long> Ids2 { get; set; }
        public List<long> Ids { get; set; }
        public long? Id { get; set; }
    }
    public class ProductFilter
    {
        public string Keyword { get; set; }
        public long? CategoryId { get; set; }
    }
}