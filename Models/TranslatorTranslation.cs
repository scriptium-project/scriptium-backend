using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class TranslatorTranslation
    {
        [Required, Column("translator_id", TypeName = DBType16bitInteger)]
        public short TranslatorId { get; set; }

        [ForeignKey("TranslatorId")]
        public virtual Translator Translator { get; set; } = null!;

        [Required, Column("translation_id", TypeName = DBType16bitInteger)]
        public short TranslationId { get; set; }

        [ForeignKey("TranslationId")]
        public virtual Translation Translation { get; set; } = null!;

        [Column("assigned_on", TypeName = DBTypeDateTime)]
        public DateTime AssignedOn { get; set; } = DateTime.UtcNow;
    }
}
