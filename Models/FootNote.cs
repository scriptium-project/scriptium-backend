using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
    public class FootNote
    {
        [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "smallint")]
        public short Number { get; set; }

        [Column(TypeName = "smallint")]
        public short Index { get; set; }

        [Required, Column("footnote_text_id", TypeName = "bigint")]
        public required long FootNoteTextId { get; set; }

        public required FootNoteText FootNoteText { get; set; }

        [Required, Column("translation_text_id",TypeName = "bigint")]
        public required long TranslationTextId { get; set; }

        public required TranslationText TranslationText { get; set; }
    }
}