using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Scripture
    {
        [Key, Column("id", TypeName = "smallint")]
        public short Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Column(TypeName = "char(1)"), MaxLength(1), Required]
        public required string Code { get; set; }

        [NotMapped]
        public short SectionCount => (short)(Sections?.Count ?? -1);

        [Required, Column(TypeName = "smallint")]
        public required short Number { get; set; }

        public List<ScriptureMeaning> Meanings { get; set; } = [];

        public List<Section> Sections { get; set; } = [];

        public List<Root> Roots { get; set; } = [];
    }
}