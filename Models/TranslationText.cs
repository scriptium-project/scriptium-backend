using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class TranslationText
    {
        [Key, Column("id", TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Text { get; set; } = null!;

        [Required, Column("translation_id", TypeName = "smallint"), ForeignKey("Translation")]
        public short TranslationId { get; set; }

        [Required]
        public virtual Translation Translation { get; set; } = null!;

        [Required, Column("verse_id", TypeName = "int"), ForeignKey("Verse")]
        public int VerseId { get; set; }

        [Required]
        public virtual Verse Verse { get; set; } = null!;

        public virtual List<FootNote> FootNotes { get; set; } = [];

    }
}
