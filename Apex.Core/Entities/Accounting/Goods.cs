using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Core.Entities.Accounting
{
    public class Goods : CodeBase
    {
        //        [Index("IX_Code", IsClustered = false, IsUnique = true), StringLength(450)]
        //        public string Code { get; set; }
        public string ReferenceCode { get; set; }
        [ForeignKey("Group")]
        public long GroupId { get; set; }
        public string Barcode { get; set; }
        public string IranCode { get; set; }
        public string Title { get; set; }
        public decimal BasePrice { get; set; }
        public long? MajorUnitId { get; set; }
        public long? MinorUnitId { get; set; }
        public decimal Ratio { get; set; }
        public decimal MinimumPoint { get; set; }
        public decimal MaximumPoint { get; set; }
        public decimal OrderPoint { get; set; }
        public MajorUnit MajorUnit { get; set; }
        public MinorUnit MinorUnit { get; set; }
        public virtual GoodsGroup Group { get; set; }
    }
}