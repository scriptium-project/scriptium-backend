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

        public Word? Word { get; set; }

        [Required, ForeignKey("Language")]
        public short LanguageId { get; set; }

        public Language? Language { get; set; }
    }
}
