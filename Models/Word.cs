using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace writings_backend_dotnet.Models
{
    public class Word
    {
        [Key, Column("id", TypeName = Utility.DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("sequence_number", TypeName = Utility.DBType16bitInteger)]
        public required short SequenceNumber { get; set; }

        [Required, Column("text", TypeName = Utility.DBTypeVARCHAR50), MaxLength(50)]
        public required string Text { get; set; }

        [Column("text_without_vowel", TypeName = Utility.DBTypeVARCHAR50), MaxLength(50)]
        public string? TextWithoutVowel { get; set; }

        [Column("text_simplified", TypeName = Utility.DBTypeVARCHAR50), MaxLength(50)]
        public string? TextSimplified { get; set; }

        [Required, Column("verse_id", TypeName = Utility.DBType32bitInteger)]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;

        [Column("root_id", TypeName = Utility.DBType64bitInteger)]
        public int? RootId { get; set; } = null!;

        public virtual Root? Root { get; set; } = null!;

        public virtual List<WordMeaning>? WordMeanings { get; set; }

    }
}