using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class WordMeaning
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("word_meaning", TypeName = DBTypeVARCHAR100), MaxLength(100)]
        public string Meaning { get; set; } = null!;

        [Required, ForeignKey("Word"), Column("word_id", TypeName = DBType64bitInteger)]
        public long WordId { get; set; }

        public virtual Word Word { get; set; } = null!;

        [Required, ForeignKey("Language"), Column("language_id", TypeName = DBType8bitInteger)]
        public byte LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;
    }
}
