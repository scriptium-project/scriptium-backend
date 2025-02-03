using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class WordConfinedDTO
    {
        public required short SequenceNumber { get; set; }
        public required string Text { get; set; }
        public string? TextWithoutVowel { get; set; }
        public string? TextSimplified { get; set; }
        public List<RootSimpleDTO>? Roots { get; set; }
    }

    public class WordRootDTO
    {
        public required short SequenceNumber { get; set; }
        public required string Text { get; set; }
        public string? TextWithoutVowel { get; set; }
        public string? TextSimplified { get; set; }
        public VerseDTO Verse { get; set; } = null!;
    }


    public static class WordExtensions
    {
        public static WordConfinedDTO ToWordConfinedDTO(this Word word)
        {
            return new WordConfinedDTO
            {
                SequenceNumber = word.SequenceNumber,
                Text = word.Text,
                TextWithoutVowel = word.TextWithoutVowel,
                TextSimplified = word.TextSimplified,
                Roots = word.Roots?.Select(r => r.ToRootSimpleDTO()).ToList() ?? null
            };
        }

        public static WordRootDTO ToWordRootDTO(this Word word)
        {
            return new WordRootDTO
            {
                SequenceNumber = word.SequenceNumber,
                Text = word.Text,
                TextWithoutVowel = word.TextWithoutVowel,
                TextSimplified = word.TextSimplified,
                Verse = word.Verse.ToVerseDTO()
            };
        }
    }
}