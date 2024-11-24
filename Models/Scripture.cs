using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Scripture
    {
        [Key, Column("id", TypeName = Utility.DBType8bitInteger)]
        public byte Id { get; set; }

        [Required, Column(TypeName = Utility.DBTypeVARCHAR50), MaxLength(50)]
        public required string Name { get; set; }

        [Column(TypeName = Utility.DBTypeCHAR1), MaxLength(1), Required]
        public required string Code { get; set; }

        [NotMapped]
        public short SectionCount => (short)(Sections?.Count ?? -1);

        [Required, Column(TypeName = Utility.DBType8bitInteger)]
        public required byte Number { get; set; }

        public virtual List<ScriptureMeaning> Meanings { get; set; } = [];

        public virtual List<Section> Sections { get; set; } = [];

        public virtual List<Root> Roots { get; set; } = [];
    }
}