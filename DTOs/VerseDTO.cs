using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class VerseSimpleDTO
    {
        public required short Number { get; set; }
        public required string Text { get; set; }
        public required string TextWithoutVowel { get; set; }
        public required string TextSimplified { get; set; }
        public required ChapterSimpleDTO Chapter { get; set; }
        public required List<WordSimpleDTO> Words { get; set; }
        public List<TransliterationSimpleDTO> Transliterations { get; set; } = [];
        public List<TranslationTextSimpleDTO> TranslationTexts { get; set; } = [];
    }

    public static class VerseExtensions
    {
        public static VerseSimpleDTO ToVerseSimpleDTO(this Verse verse)
        {
            return new VerseSimpleDTO
            {
                Number = verse.Number,
                Text = verse.Text,
                TextWithoutVowel = verse.TextWithoutVowel,
                TextSimplified = verse.TextSimplified,
                Chapter = verse.Chapter.ToChapterSimpleDTO(),
                Words = verse.Words.Select(e => e.ToWordSimpleDTO()).ToList(),
                Transliterations = verse.Transliterations.Select(e => e.ToTransliterationSimpleDTO()).ToList(),
                TranslationTexts = verse.TranslationTexts.Select(e => e.ToTranslationTextSimpleDTO()).ToList()
            };
        }
    }
}