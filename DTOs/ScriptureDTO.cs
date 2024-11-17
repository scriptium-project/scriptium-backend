using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class ScriptureSimpleDTO
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required short Number { get; set; }
        public List<ScriptureMeaningSimpleDTO> Meanings { get; set; } = [];
    }
    public class ScriptureMeaningSimpleDTO
    {
        public required string Meaning { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
    }

    public static class ScriptureExtensions
    {

        public static ScriptureSimpleDTO ToScriptureSimpleDTO(this Scripture scripture)
        {
            return new ScriptureSimpleDTO
            {
                Name = scripture.Name,
                Code = scripture.Code,
                Number = scripture.Number,
                Meanings = scripture.Meanings.Select(e => e.ToScriptureMeaningSimpleDTO()).ToList()
            };
        }
        public static ScriptureMeaningSimpleDTO ToScriptureMeaningSimpleDTO(this ScriptureMeaning scriptureMeaning)
        {
            return new ScriptureMeaningSimpleDTO
            {
                Meaning = scriptureMeaning.Meaning,
                Language = scriptureMeaning.Language.ToLanguageSimpleDTO()
            };
        }
    }
}