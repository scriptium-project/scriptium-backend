using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Translator
    {
        [Key, Column("id", TypeName = DBType16bitInteger)]
        public short Id { get; set; }

        [Required, Column("name", TypeName = DBTypeVARCHAR250), MaxLength(250)]
        public required string Name { get; set; } = null!;

        [Column("description", TypeName = DBTypeVARCHARMAX)]
        public string? Description { get; set; }

        [Column("url", TypeName = DBTypeVARCHARMAX)]
        public string? Url { get; set; }

        [Required, Column("language_id"), ForeignKey("Language")]
        public byte LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;

        public virtual List<TranslatorTranslation>? TranslatorTranslations { get; set; }
    }
}
