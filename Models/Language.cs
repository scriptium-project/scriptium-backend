

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Language
    {
        [Key, Column("id", TypeName = DBType8bitInteger)]
        public byte Id { get; set; }

        [Column(TypeName = DBTypeVARCHAR16)]
        public required string LangEnglish { get; set; }

        [Column(TypeName = DBTypeVARCHAR16)]
        public required string LangOwn { get; set; }

        [Column(TypeName = DBTypeVARCHAR2)]
        public required string LangCode { get; set; }

        public virtual List<ScriptureMeaning>? ScriptureMeanings { get; set; }

        public virtual List<SectionMeaning>? SectionMeanings { get; set; }

        public virtual List<ChapterMeaning>? ChapterMeanings { get; set; }

        public virtual List<WordMeaning>? WordMeanings { get; set; }

        public virtual List<Transliteration>? Transliterations { get; set; }

        public virtual List<Translator>? Translators { get; set; }

        public virtual List<Translation>? Translations { get; set; }

        public virtual List<User>? PreferredUsers { get; set; }


    }
}