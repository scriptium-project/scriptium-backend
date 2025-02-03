using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class TranslationDTO
    {
        public required short Id { get; set; }
        public required string Name { get; set; }
        public required LanguageDTO Language { get; set; }
        public required List<TranslatorDTO> Translators { get; set; }
        public required bool IsEager { get; set; }
    }

    public class TranslationExpendedDTO : TranslationDTO
    {
        public required ScriptureDTO Scripture { get; set; }

    }

    public static class TranslationExtensions
    {
        public static TranslationDTO ToTranslationDTO(this Translation translation)
        {
            return new TranslationDTO
            {
                Id = translation.Id,
                Name = translation.Name,
                Language = translation.Language.ToLanguageDTO(),
                Translators = translation.TranslatorTranslations.Select(e => e.Translator.ToTranslatorDTO()).ToList(),
                IsEager = translation.EagerFrom.HasValue
            };
        }

        public static TranslationExpendedDTO ToTranslationExpendedDTO(this Translation translation)
        {
            return new TranslationExpendedDTO
            {
                Id = translation.Id,
                Name = translation.Name,
                Language = translation.Language.ToLanguageDTO(),
                Translators = translation.TranslatorTranslations.Select(e => e.Translator.ToTranslatorDTO()).ToList(),
                IsEager = translation.EagerFrom.HasValue,
                Scripture = translation.Scripture.ToScriptureDTO()
            };
        }
    }
}