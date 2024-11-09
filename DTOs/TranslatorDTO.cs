using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class TranslatorSimpleDTO
    {
        public required string Name { get; set; }
        public string? URL { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
    }

    public static class TranslatorExtensions
    {
        public static TranslatorSimpleDTO ToTranslatorSimpleDTO(this Translator translator)
        {
            return new TranslatorSimpleDTO
            {
                Name = translator.Name,
                URL = translator.Url,
                Language = translator.Language.ToLanguageSimpleDTO()
            };
        }
    }
}