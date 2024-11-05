

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace writings_backend_dotnet.Models
{
    public class Scripture
    {
        [Key, Column("id", TypeName = "smallint")]
        public short Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Column(TypeName = "char(1)"), MaxLength(1), Required]
        public required int Code { get; set; }

        [NotMapped]
        public int SectionCount => Sections.Count;

        [Required]
        public required int ScriptureMeaningId { get; set; }

        public List<ScriptureMeaning> Meanings { get; set; } = [];
        
        public List<Section> Sections = [];

        public List<Root> Roots = [];
    }
}