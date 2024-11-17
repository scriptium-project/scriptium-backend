using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class ChapterMeaning 
    {
        [Key, Column("id", TypeName = DBType32bitInteger)]
        public int Id { get; set; }

        [Column("meaning", TypeName = DBTypeVARCHAR100)]
        public required string Meaning { get; set; }

        [Required, Column("chapter_id", TypeName = DBType16bitInteger)]
        public short ChapterId { get; set; }
        public virtual Chapter Chapter { get; set; } = null!;

        [Required, Column("language_id", TypeName = DBType8bitInteger)]
        public byte LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;
    }
}