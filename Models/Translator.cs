using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Translator
    {
        [Key, Column("id", TypeName = "smallint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required , Column("translator_name", TypeName = "varchar(250)") , MaxLength(250)]
        public string Name { get; set; } = null!;

        [Column("description", TypeName = "varchar(1500)"), MaxLength(1500)]
        public string? Description { get; set; }

        [Column(TypeName = "varchar(1500)"), MaxLength(1500)]
        public string? Url { get; set; }

        [Required, Column("language_id"), ForeignKey("Language")]
        public short LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;

        public virtual ICollection<TranslatorTranslation> TranslatorTranslations { get; set; } = [];
    }
}
