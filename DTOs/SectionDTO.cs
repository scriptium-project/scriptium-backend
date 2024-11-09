using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class SectionSimpleDTO
    {
        public required string Name { get; set; }
        public required short Number { get; set; }
        public required ScriptureSimpleDTO Scripture { get; set; }
        public required List<SectionMeaningSimpleDTO> Meanings { get; set; } = [];

    }

    public class SectionMeaningSimpleDTO
    {
        public required string Meaning { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
    }

    public static class SectionExtensions
    {
        public static SectionSimpleDTO ToSectionSimpleDTO(this Section section)
        {
            return new SectionSimpleDTO
            {
                Name = section.Name,
                Number = section.Number,
                Scripture = section.Scripture.ToScriptureSimpleDTO(),
                Meanings = section.Meanings.Select(e => e.ToSectionMeaningSimpleDTO()).ToList()
            };
        }

        public static SectionMeaningSimpleDTO ToSectionMeaningSimpleDTO(this SectionMeaning sectionMeaning)
        {
            return new SectionMeaningSimpleDTO
            {
                Meaning = sectionMeaning.Meaning,
                Language = sectionMeaning.Language.ToLanguageSimpleDTO()
            };
        }
    }

}