using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class TranslationTextDTO
    {
        public required string Text { get; set; }
        public required TranslationDTO Translation { get; set; }
        public required List<FootNoteDTO> FootNotes { get; set; }

    }

    public class TranslationTextExtendedVerseDTO
    {
        public required string Text { get; set; }
        public required TranslationDTO Translation { get; set; }
        public required List<FootNoteDTO> FootNotes { get; set; }
        public required VerseSimpleDTO Verse { get; set; }

    }


    public class TranslationWithMultiTextDTO
    {
        public required TranslationDTO Translation { get; set; }
        public required List<TranslationTextSimpleDTO> TranslationTexts { get; set; }

    }

    public class TranslationWithSingleTextDTO
    {
        public required TranslationDTO Translation { get; set; }
        public required TranslationTextSimpleDTO TranslationText { get; set; }
    }

    public class TranslationTextSimpleDTO
    {
        public required string Text { get; set; }
        public required List<FootNoteDTO> FootNotes { get; set; }

    }

    public static class TranslationTextExtensions
    {
        public static TranslationTextDTO ToTranslationTextDTO(this TranslationText translationText)
        {
            return new TranslationTextDTO
            {
                Text = translationText.Text,
                Translation = translationText.Translation.ToTranslationDTO(),
                FootNotes = translationText.FootNotes.Select(e => e.ToFootNoteDTO()).ToList(),
            };
        }

        public static TranslationTextExtendedVerseDTO ToTranslationTextExtendedVerseDTO(this TranslationText translationText)
        {
            return new TranslationTextExtendedVerseDTO
            {
                Text = translationText.Text,
                Translation = translationText.Translation.ToTranslationDTO(),
                FootNotes = translationText.FootNotes.Select(e => e.ToFootNoteDTO()).ToList(),
                Verse = translationText.Verse.ToVerseSimpleDTO()
            };
        }
    }
}