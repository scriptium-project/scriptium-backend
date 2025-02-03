using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class ScriptureConfinedDTO
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required short Number { get; set; }
        public List<ScriptureMeaningDTO> Meanings { get; set; } = [];
    }
    public class ScriptureMeaningDTO
    {
        public required string Meaning { get; set; }
        public required LanguageDTO Language { get; set; }
    }

    public class ScriptureDTO
    {
        public required short Id { get; set; }

        public required string Name { get; set; }

        public required string Code { get; set; }

        public required short Number { get; set; }

        public required List<ScriptureMeaningDTO> Meanings { get; set; }

        public required List<SectionWithMeaningDTO> Sections { get; set; }

    }

    public static class ScriptureExtensions
    {

        public static ScriptureConfinedDTO ToScriptureConfinedDTO(this Scripture Scripture)
        {
            return new ScriptureConfinedDTO
            {
                Name = Scripture.Name,
                Code = Scripture.Code,
                Number = Scripture.Number,
                Meanings = Scripture.Meanings.Select(e => e.ToScriptureMeaningDTO()).ToList()
            };
        }
        public static ScriptureMeaningDTO ToScriptureMeaningDTO(this ScriptureMeaning ScriptureMeaning)
        {
            return new ScriptureMeaningDTO
            {
                Meaning = ScriptureMeaning.Meaning,
                Language = ScriptureMeaning.Language.ToLanguageDTO()
            };
        }

        public static ScriptureDTO ToScriptureDTO(this Scripture Scripture)
        {
            return new ScriptureDTO
            {
                Id = Scripture.Id,
                Name = Scripture.Name,
                Code = Scripture.Code,
                Number = Scripture.Number,
                Meanings = Scripture.Meanings.Select(m => m.ToScriptureMeaningDTO()).ToList(),
                Sections = Scripture.Sections.Select(m => m.ToSectionWithMeaningDTO()).ToList()
            };
        }
    }
}