using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace writings_backend_dotnet.Models
{
    public class Section
    {
        [Key,  Column("id", TypeName = "smallint")]
        public short Id { get; set; }

        [Required, Column("section_name",TypeName = "varchar(100)")]
        public required string Name { get; set; }

        [Required, Column("section_number",TypeName = "smallint")]
        public required int Number { get; set; }

        [NotMapped]
        public int ChapterCount => Chapters.Count;

        [Required, Column("scripture_id", TypeName = "smallint")]
        public required short ScriptureId { get; set; }

        public required Scripture Scripture { get; set; }

        public List<Chapter> Chapters { get; set; } = [];

        public List<SectionMeaning> Meanings { get; set; } = [];
        
    }
}