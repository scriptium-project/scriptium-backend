using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Translation
    {
        [Key, Column("id", TypeName = DBType16bitInteger)]
        public short Id { get; set; }

        [Required, Column("name", TypeName = DBTypeVARCHAR250), MaxLength(300)]
        public required string Name { get; set; }

        [Column("production_year")]
        public DateTime? ProductionTime { get; set; }

        [Column("added_at")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [Column("eager_from", TypeName = DBTypeDateTime)]
        public DateTime? EagerFrom { get; set; }

        [Required, ForeignKey("Language"), Column("language_id", TypeName = DBType8bitInteger)]
        public byte LanguageId { get; set; } = 1;

        public virtual Language Language { get; set; } = null!;

        public virtual List<TranslatorTranslation> TranslatorTranslations { get; set; } = [];

        public virtual List<TranslationText> TranslationTexts { get; set; } = [];
    }
}
