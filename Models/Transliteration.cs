using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Transliteration
    {
        [Key, Column("id", TypeName = DBType32bitInteger)]
        public int Id { get; set; }

        [Required, Column("text",TypeName = DBTypeVARCHARMAX)]
        public required string Text { get; set; }

        [Required, Column("language_id", TypeName = DBType8bitInteger), ForeignKey("Language")]
        public byte LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;

        [Required, Column("verse_id", TypeName = DBType32bitInteger), ForeignKey("Verse")]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;
    }
}
