using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Accounting.Core.Entities.Accounting
{
    public class PersonGroupPin
    {
        [Key, Column(Order = 1)]
        public long PersonId { get; set; }
        [Key, Column(Order = 2)]
        public long GroupId { get; set; }

        public Person Person { get; set; }
        public PersonGroup Group { get; set; }
    }
}