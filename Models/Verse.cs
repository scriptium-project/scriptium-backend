using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace writings_backend_dotnet.Models
{
    public class Verse
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, Column("number", TypeName = Utility.DBType16bitInteger)]
        public required short Number { get; set; }

        [Required, Column("text", TypeName = Utility.DBTypeVARCHARMAX)]
        public required string Text { get; set; }

        [Column("text", TypeName = Utility.DBTypeVARCHARMAX)]
        public required string TextWithoutVowel { get; set; }

        [Column("text", TypeName = Utility.DBTypeVARCHARMAX)]
        public string TextSimplified { get; set; } = null!;

        [Required, Column("chapter_id", TypeName = Utility.DBType16bitInteger)]
        public short ChapterId { get; set; }

        public virtual Chapter Chapter { get; set; } = null!;

        public virtual List<Word> Words { get; set; } = [];

        public virtual List<Transliteration> Transliterations { get; set; } = [];

        public virtual List<TranslationText> TranslationTexts { get; set; } = [];

        public virtual List<CollectionVerse> CollectionVerses { get; set; } = [];

        public virtual List<Note> Notes { get; set; } = [];

        public virtual List<CommentVerse> Comments { get; set; } = [];


    }
}