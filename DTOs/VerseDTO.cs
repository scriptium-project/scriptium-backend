using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class VerseDTO
    {
        public required int Id { get; set; }

        public required short Number { get; set; }

        public required string Text { get; set; }

        public required string? TextWithoutVowel { get; set; }

        public required string? TextSimplified { get; set; }

        public required ChapterDTO Chapter { get; set; }

        public required List<WordConfinedDTO> Words { get; set; }

        public List<TransliterationDTO> Transliterations { get; set; } = [];

        public List<TranslationTextDTO> TranslationTexts { get; set; } = [];

        public bool IsSaved { get; set; } = false;

    }

    public class VerseSimpleDTO
    {
        public required int Id { get; set; }

        public required short Number { get; set; }

        public required string Text { get; set; }

        public required string? TextWithoutVowel { get; set; }

        public required string? TextSimplified { get; set; }

        public required ChapterDTO Chapter { get; set; }

    }

    public class VerseExpendedWordDTO
    {
        public required int Id { get; set; }

        public required string Text { get; set; }

        public required string? TextWithoutVowel { get; set; }

        public required string? TextSimplified { get; set; }

        public required int Number { get; set; }

        public required ChapterDTO Chapter { get; set; }

        public required List<TransliterationDTO>? Transliterations { get; set; }

        public required List<WordConfinedDTO> Words { get; set; }

    }

    public class VerseCollectionDTO
    {
        public required int Id { get; set; }

        public required short Number { get; set; }

        public required string Text { get; set; }

        public required string? TextWithoutVowel { get; set; }

        public required string? TextSimplified { get; set; }

        public required int ChapterNumber { get; set; }

        public required SectionDTO Section { get; set; }

        public List<TransliterationDTO> Transliterations { get; set; } = [];

        public List<TranslationWithSingleTextDTO> Translations { get; set; } = [];

    }

    public class ConfinedVerseDTO
    {
        public required int Id { get; set; }

        public required string Text { get; set; }

        public required string? TextSimplified { get; set; }

        public required string? TextWithoutVowel { get; set; }

        public required int Number { get; set; }

        public required List<TransliterationDTO>? Transliterations { get; set; }

    }


    public static class VerseExtensions
    {
        public static VerseDTO ToVerseDTO(this Verse verse)
        {
            return new VerseDTO
            {
                Id = verse.Id,
                Number = verse.Number,
                Text = verse.Text,
                TextWithoutVowel = verse.TextWithoutVowel,
                TextSimplified = verse.TextSimplified,
                Chapter = verse.Chapter.ToChapterDTO(),
                Words = verse.Words.Select(e => e.ToWordConfinedDTO()).ToList(),
                Transliterations = verse.Transliterations.Select(e => e.ToTransliterationDTO()).ToList(),
                TranslationTexts = verse.TranslationTexts.Select(e => e.ToTranslationTextDTO()).ToList(),
            };
        }

        public static VerseSimpleDTO ToVerseSimpleDTO(this Verse verse)
        {
            return new VerseSimpleDTO
            {
                Id = verse.Id,
                Number = verse.Number,
                Text = verse.Text,
                TextWithoutVowel = verse.TextWithoutVowel,
                TextSimplified = verse.TextSimplified,
                Chapter = verse.Chapter.ToChapterDTO(),
            };
        }

        public static VerseExpendedWordDTO ToVerseExpendedWordDTO(this Verse verse)
        {
            return new VerseExpendedWordDTO
            {
                Id = verse.Id,
                Text = verse.Text,
                Chapter = verse.Chapter.ToChapterDTO(),
                TextSimplified = verse.TextSimplified,
                TextWithoutVowel = verse.TextWithoutVowel,
                Number = verse.Number,
                Transliterations = verse.Transliterations.Select(t => t.ToTransliterationDTO()).ToList(),
                Words = verse.Words.Select(w => w.ToWordConfinedDTO()).ToList()

            };
        }

    }

}




