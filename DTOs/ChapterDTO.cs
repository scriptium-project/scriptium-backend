using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class ChapterSimpleDTO
    {
        public required string Name { get; set; }
        public required short Number { get; set; }
        public required SectionSimpleDTO Section { get; set; }

        public required List<ChapterMeaningSimpleDTO> Meanings { get; set; } = [];

    }
    public class ChapterMeaningSimpleDTO
    {
        public required string Meaning { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
    }

    public static class ChapterExtensions
    {
        public static ChapterSimpleDTO ToChapterSimpleDTO(this Chapter chapter)
        {
            return new ChapterSimpleDTO
            {
                Name = chapter.Name,
                Number = chapter.Number,
                Section = chapter.Section.ToSectionSimpleDTO(),
                Meanings = chapter.Meanings.Select(e => e.ToChapterMeaningSimpleDTO()).ToList()

            };
        }

        public static ChapterMeaningSimpleDTO ToChapterMeaningSimpleDTO(this ChapterMeaning chapterMeaning)
        {
            return new ChapterMeaningSimpleDTO
            {
                Meaning = chapterMeaning.Meaning,
                Language = chapterMeaning.Language.ToLanguageSimpleDTO()
            };
        }
    }
}