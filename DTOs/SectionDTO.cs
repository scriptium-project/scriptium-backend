using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class SectionDTO
    {
        public required string Name { get; set; }
        public required short Number { get; set; }
        public required ScriptureConfinedDTO Scripture { get; set; }
        public required List<SectionMeaningDTO> Meanings { get; set; } = [];

    }

    public class SectionMeaningDTO
    {
        public required string Meaning { get; set; }
        public required LanguageDTO Language { get; set; }
    }

    public class SectionWithMeaningDTO
    {
        public required string Name { get; set; }

        public required List<SectionMeaningDTO> Meanings { get; set; }

    }

    public class SectionSimpleDTO
    {
        public required string Name { get; set; }

        public required short ChapterCount { get; set; }

        public required string ScriptureName { get; set; }

        public required List<ScriptureMeaningDTO> ScriptureMeanings { get; set; }

        public required List<SectionMeaningDTO> SectionMeanings { get; set; }

    }

    public static class SectionExtensions
    {
        public static SectionDTO ToSectionDTO(this Section section)
        {
            return new SectionDTO
            {
                Name = section.Name,
                Number = section.Number,
                Scripture = section.Scripture.ToScriptureConfinedDTO(),
                Meanings = section.Meanings.Select(e => e.ToSectionMeaningDTO()).ToList()
            };
        }

        public static SectionMeaningDTO ToSectionMeaningDTO(this SectionMeaning sectionMeaning)
        {
            return new SectionMeaningDTO
            {
                Meaning = sectionMeaning.Meaning,
                Language = sectionMeaning.Language.ToLanguageDTO()
            };
        }

        public static SectionWithMeaningDTO ToSectionWithMeaningDTO(this Section Section)
        {
            return new SectionWithMeaningDTO
            {
                Name = Section.Name,
                Meanings = Section.Meanings.Select(m => m.ToSectionMeaningDTO()).ToList(),
            };
        }

        public static SectionSimpleDTO ToSectiomSimpleDTO(this Section section)
        {
            return new SectionSimpleDTO
            {
                ScriptureName = section.Scripture.Name,
                ScriptureMeanings = section.Scripture.Meanings.Select(m => new ScriptureMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),
                Name = section.Name,
                SectionMeanings = section.Meanings.Select(m => new SectionMeaningDTO { Language = m.Language.ToLanguageDTO(), Meaning = m.Meaning }).ToList(),
                ChapterCount = section.ChapterCount,
            };
        }
    }

}