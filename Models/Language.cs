

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Language
    {
        [Key, Column("id", TypeName = "smallint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Column(TypeName = "varchar(20)")]
        public required string LangEnglish { get; set; }

        [Column(TypeName = "varchar(20)")]
        public required string LangOwn { get; set; }

        [Column(TypeName = "varchar(2)")]
        public required string LangCode { get; set; }

        public List<ScriptureMeaning> ScriptureMeanings { get; set; } = [];

        public List<SectionMeaning> SectionMeanings { get; set; } = [];

        public List<ChapterMeaning> ChapterMeaning { get; set; } = [];

        public List<WordMeaning> WordMeanings { get; set; } = [];

        public List<Transliteration> Transliterations { get; set; } = [];

        public List<Translator> Translators { get; set; } = [];

        public List<Translation> Translations { get; set; } = [];

        public List<User> PreferredUsers { get; set; } = [];


    }
}