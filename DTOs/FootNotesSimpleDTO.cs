using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class FootNoteSimpleDTO
    {
        public required short Number { get; set; }
        public required short Index { get; set; }
        public required string Text { get; set; }
    }

    public static class FootNoteSimpleExtensions
    {
        public static FootNoteSimpleDTO ToFootNoteSimpleDTO(this FootNote footNote)
        {
            return new FootNoteSimpleDTO
            {
                Number = footNote.Number,
                Index = footNote.Index,
                Text = footNote.FootNoteText.Text
            };
        }
    }
}