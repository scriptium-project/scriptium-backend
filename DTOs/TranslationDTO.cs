using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class TranslationSimpleDTO
    {
        public required string Name { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
        public required List<TranslatorSimpleDTO> Translators { get; set; }
    }

    public static class TranslationExtensions
    {
        public static TranslationSimpleDTO ToTranslationSimpleDTO(this Translation translation)
        {
            return new TranslationSimpleDTO
            {
                Name = translation.Name,
                Language = translation.Language.ToLanguageSimpleDTO(),
                Translators = translation.TranslatorTranslations.Select(e => e.Translator.ToTranslatorSimpleDTO()).ToList()
            };
        }
    }
}