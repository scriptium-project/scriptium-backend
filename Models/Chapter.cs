using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;


namespace writings_backend_dotnet.Models
{
    public class Chapter
    {
        [Key, Column("id", TypeName = DBType16bitInteger)]
        public short Id { get; set; }

        [Required, Column("name", TypeName = DBTypeVARCHAR100)]
        public required string Name { get; set; }

        [Required, Column("number", TypeName = DBType8bitInteger)]
        public required byte Number { get; set; }

        [NotMapped]
        public short VerseCount => (short)(Verses?.Count ?? -1);

        [Required, Column("section_id", TypeName = DBType16bitInteger)]
        public short SectionId { get; set; }

        public virtual Section Section { get; set; } = null!;

        public virtual List<Verse> Verses { get; set; } = [];

        public virtual List<ChapterMeaning> Meanings { get; set; } = [];

    }
}