using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class TranslatorTranslation
    {
        [Required, Column("translator_id", TypeName = "smallint")]
        public short TranslatorId { get; set; }

        [Required, ForeignKey("TranslatorId")]
        public required Translator Translator { get; set; }

        [Required, Column("translation_id", TypeName = "smallint")]
        public short TranslationId { get; set; }

        [Required, ForeignKey("TranslationId")]
        public required Translation Translation { get; set; }

        [Column("assigned_on")]
        public DateTime AssignedOn { get; set; }
    }
}
