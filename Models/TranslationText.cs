using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class TranslationText
    {
        [Key, Column("id", TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required string Text { get; set; }

        [Required, Column("translation_id", TypeName = "smallint"), ForeignKey("Translation")]
        public short TranslationId { get; set; }

        [Required]
        public required Translation Translation { get; set; }

        [Required, Column("verse_id", TypeName = "int"), ForeignKey("Verse")]
        public int VerseId { get; set; }

        public required Verse Verse { get; set; }

        public List<FootNote> FootNotes { get; set; } = [];

    }
}
