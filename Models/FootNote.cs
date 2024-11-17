using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class FootNote
    {
        [Key, Column("id")]
        public long Id { get; set; }

        [Column(TypeName = DBType16bitInteger)]
        public short Number { get; set; }

        [Column(TypeName = DBType16bitInteger)]
        public short Index { get; set; }

        [Required, Column("footnote_text_id", TypeName = DBType64bitInteger)]
        public long FootNoteTextId { get; set; }

        public virtual FootNoteText FootNoteText { get; set; } = null!;

        [Required, Column("translation_text_id", TypeName = DBType64bitInteger)]
        public required long TranslationTextId { get; set; }

        public virtual TranslationText TranslationText { get; set; }  = null!;
    }
}