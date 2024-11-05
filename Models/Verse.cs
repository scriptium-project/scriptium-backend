using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace writings_backend_dotnet.Models
{
    public class Verse
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, Column("verse_number", TypeName = "smallint")]
        public required int Number { get; set; }

        [Required]
        public required string Text { get; set; }

        [Required]
        public required string TextNoVowel { get; set; }

        public string TextSimplified { get; set; } = null!;

        [Required]
        public required int ChapterId { get; set; }

        public required Chapter Chapter { get; set; }

        public virtual List<Word> Words { get; set; } = [];

        public virtual List<Transliteration> Transliterations { get; set; } = [];

        public virtual List<TranslationText> TranslationTexts { get; set; } = [];

        public virtual List<CollectionVerse> CollectionVerses { get; set; } = [];

        public virtual List<Note> Notes { get; set; } = [];
        
        public virtual List<CommentVerse> Comments { get; set; } = [];

        
    }
}