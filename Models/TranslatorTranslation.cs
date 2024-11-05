using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class TranslatorTranslation
    {
        [Required, Column("translator_id", TypeName = "smallint")]
        public short TranslatorId { get; set; }

        [Required, ForeignKey("TranslatorId")]
        public virtual Translator Translator { get; set; } = null!;

        [Required, Column("translation_id", TypeName = "smallint")]
        public short TranslationId { get; set; }

        [Required, ForeignKey("TranslationId")]
        public virtual Translation Translation { get; set; } = null!;

        [Column("assigned_on")]
        public DateTime AssignedOn { get; set; }
    }
}
