using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class ChapterDTO
    {
        public required string Name { get; set; }
        public required short Number { get; set; }
        public required SectionDTO Section { get; set; }
        public required List<ChapterMeaningSimpleDTO> Meanings { get; set; } = [];

    }
    public class ChapterMeaningSimpleDTO
    {
        public required string Meaning { get; set; }
        public required LanguageDTO Language { get; set; }
    }

    public class ChapterConfinedDTO
    {
        public required string ScriptureName { get; set; }

        public required List<ScriptureMeaningDTO> ScriptureMeanings { get; set; }

        public required string SectionName { get; set; }

        public required List<SectionMeaningDTO> SectionMeanings { get; set; }

        public required string ChapterName { get; set; }

        public required short ChapterNumber { get; set; }
        public required List<ConfinedVerseDTO> Verses { get; set; }

        public required List<TranslationWithMultiTextDTO> Translations { get; set; }

    }

    public static class ChapterExtensions
    {
        public static ChapterDTO ToChapterDTO(this Chapter chapter)
        {
            return new ChapterDTO
            {
                Name = chapter.Name,
                Number = chapter.Number,
                Section = chapter.Section.ToSectionDTO(),
                Meanings = chapter.Meanings.Select(e => e.ToChapterMeaningSimpleDTO()).ToList()

            };
        }

        public static ChapterMeaningSimpleDTO ToChapterMeaningSimpleDTO(this ChapterMeaning chapterMeaning)
        {
            return new ChapterMeaningSimpleDTO
            {
                Meaning = chapterMeaning.Meaning,
                Language = chapterMeaning.Language.ToLanguageDTO()
            };
        }

    }
}