using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Translation
    {
        [Key, Column("id", TypeName = "smallint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required, Column("translation_name", TypeName = "varchar(300)"), MaxLength(300)]
        public string Name { get; set; } = null!;

        [Column("production_year")]
        public DateTime? ProductionTime { get; set; }

        [Column("added_at")]
        public DateTime AddedAt { get; set; }

        [Column("eager_from")]
        public DateTime? EagerFrom { get; set; }

        [Required, ForeignKey("Language"), Column("language_id")]
        public short LanguageId { get; set; } = 1;

        public required Language Language { get; set; }

        public required List<TranslatorTranslation> TranslatorTranslations { get; set; } = [];

        public required List<TranslationText> TranslationTexts { get; set; } = [];
    }
}
