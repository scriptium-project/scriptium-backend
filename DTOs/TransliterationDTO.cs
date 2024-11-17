using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class TransliterationSimpleDTO
    {
        public required string Transliteration { get; set; }
        public required LanguageSimpleDTO Language { get; set; }
    }

    public static class TransliterationExtensions
    {
        public static TransliterationSimpleDTO ToTransliterationSimpleDTO(this Transliteration transliteration)
        {
            return new TransliterationSimpleDTO
            {
                Transliteration = transliteration.Text,
                Language = transliteration.Language.ToLanguageSimpleDTO()
            };
        }
    }
}