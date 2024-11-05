using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace writings_backend_dotnet.Models
{
    public class Chapter
    {
        [Key,Column("id")]
        public int Id { get; set; }

        [Required, Column("chapter_name", TypeName = "varchar(250)")]
        public required string Name { get; set; }

        [Required, Column("chapter_number", TypeName = "smallint")]
        public required short Number { get; set; }

        [NotMapped]
        public int VerseCount => Verses.Count;

        [Required, Column("section_id", TypeName = "smallint")]
        public required short SectionId { get; set; }

        public required Section Section { get; set; }

        public List<Verse> Verses { get; set; } = [];
        
        public List<ChapterMeaning> Meanings { get; set; } = [];

    }
}