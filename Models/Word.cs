using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
    public class Word
    {
        [Key, Column("id", TypeName = "bigint")]
        public long Id { get; set; }

        [Required, Column("sequence_number",TypeName = "smallint")]
        public required int SequenceNumber { get; set; }

        [Required]
        public required string Text { get; set; }

        public string? TextNoVowel { get; set; } = null!;

        public string? TextSimplified { get; set; } = null!;

        [Required]
        public int VerseId { get; set; }

        public required Verse Verse { get; set; }

        public int? RootId { get; set; } = null!;

        public Root? Root { get; set; } = null!;

        public required List<WordMeaning> WordMeanings { get; set; }
        

    }
}