using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class WordMeaning
    {
        [Key, Column("id", TypeName = "bigint")]
        public long Id { get; set; }

        [Required, Column("word_meaning", TypeName = "varchar(100)"), MaxLength(100)]
        public string Meaning { get; set; } = null!;

        [Required, ForeignKey("Word")]
        public long WordId { get; set; }

        public virtual Word Word { get; set; } = null!;

        [Required, ForeignKey("Language")]
        public short LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;
    }
}
