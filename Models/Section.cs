using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;


namespace writings_backend_dotnet.Models
{
    public class Section
    {
        [Key, Column("id", TypeName = DBType16bitInteger)]
        public short Id { get; set; }

        [Required, Column("name", TypeName = DBTypeVARCHAR100)]
        public required string Name { get; set; }

        [Required, Column("number", TypeName = DBType16bitInteger)]
        public required short Number { get; set; }

        [NotMapped]
        public short ChapterCount => (short)(Chapters?.Count ?? -1);

        [Required, Column("scripture_id", TypeName = DBType8bitInteger)]
        public byte ScriptureId { get; set; }

        public virtual Scripture Scripture { get; set; } = null!;

        public virtual List<Chapter> Chapters { get; set; } = [];

        public virtual List<SectionMeaning> Meanings { get; set; } = [];

    }
}