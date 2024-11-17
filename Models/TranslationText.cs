using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class TranslationText
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("text", TypeName = DBTypeVARCHARMAX)]
        public required string Text { get; set; }

        [Required, Column("translation_id", TypeName = DBType16bitInteger), ForeignKey("Translation")]
        public short TranslationId { get; set; }

        public virtual Translation Translation { get; set; } = null!;

        [Required, Column("verse_id", TypeName = DBType32bitInteger), ForeignKey("Verse")]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;

        public virtual List<FootNote> FootNotes { get; set; } = [];

        public virtual List<Suggestion>? Suggestions { get; set; }

    }
}
