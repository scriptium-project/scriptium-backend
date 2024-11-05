using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Transliteration
    {
        [Key, Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "varchar(2500)"), MaxLength(2500)]
        public string Text { get; set; } = null!;

        [Required, ForeignKey("Language")]
        public short LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;

        public int? VerseId { get; set; }

        public virtual Verse? Verse { get; set; }
    }
}
