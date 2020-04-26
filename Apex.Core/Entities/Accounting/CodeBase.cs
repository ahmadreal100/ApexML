using Accounting.Core.Abstract;

namespace Accounting.Core.Entities.Accounting
{
    public class CodeBase : Entity
    {
        //        [Index("IX_Code", IsClustered = false, IsUnique = true), StringLength(450)]
        public string Code { get; set; }
    }
}
