using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class LanguageSimpleDTO
    {
        public string LangCode { get; set; } = string.Empty;
        public string LangOwn { get; set; } = string.Empty;
        public string LangEnglish { get; set; } = string.Empty;
    }

    public static class LanguageExtensions
    {
        public static LanguageSimpleDTO ToLanguageSimpleDTO(this Language language)
        {
            return new LanguageSimpleDTO
            {
                LangCode = language.LangCode,
                LangOwn = language.LangOwn,
                LangEnglish = language.LangEnglish
            };
        }
    }
}
