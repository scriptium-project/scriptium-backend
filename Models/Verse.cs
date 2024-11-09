using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Verse
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, Column("verse_number", TypeName = "smallint")]
        public required short Number { get; set; }

        [Required]
        public required string Text { get; set; }

        [Required]
        public required string TextNoVowel { get; set; }

        public string TextSimplified { get; set; } = null!;

        [Required]
        public required int ChapterId { get; set; }

        public required Chapter Chapter { get; set; }

        public List<Word> Words { get; set; } = [];

        public List<Transliteration> Transliterations { get; set; } = [];

        public List<TranslationText> TranslationTexts { get; set; } = [];

        public List<CollectionVerse> CollectionVerses { get; set; } = [];

        public List<Note> Notes { get; set; } = [];

        public List<CommentVerse> Comments { get; set; } = [];


    }
}