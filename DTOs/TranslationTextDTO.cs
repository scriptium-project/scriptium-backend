using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class TranslationTextSimpleDTO
    {
        public required string Text { get; set; }
        public required TranslationSimpleDTO Translation { get; set; }
        public required List<FootNoteSimpleDTO> FootNotes { get; set; }


    }

    public static class TranslationTextExtensions
    {
        public static TranslationTextSimpleDTO ToTranslationTextSimpleDTO(this TranslationText translationText)
        {
            return new TranslationTextSimpleDTO
            {
                Text = translationText.Text,
                Translation = translationText.Translation.ToTranslationSimpleDTO(),
                FootNotes = translationText.FootNotes.Select(e => e.ToFootNoteSimpleDTO()).ToList(),
            };
        }
    }
}