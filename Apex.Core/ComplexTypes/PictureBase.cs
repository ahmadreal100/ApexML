using Apex.Core.Abstract;

namespace Apex.Core.ComplexTypes
{
    public class PictureBase : Entity
    {
        public string Link { get; set; }
        public int DisplayOrder { get; set; }
    }
}