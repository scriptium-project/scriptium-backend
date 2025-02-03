using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{

    public class NoteDTO
    {
        public required long Id { get; set; }

        public required string NoteText { get; set; }

        public required UserDTO User { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime? UpdatedAt { get; set; }

        public required int LikeCount { get; set; }

        public required int ReplyCount { get; set; }

        public bool IsLiked { get; set; } = false;
    }

    public class NoteDTOExtended : NoteDTO
    {
        public required VerseSimpleDTO Verse { get; set; }
    }

    public class LikedNoteDTO
    {
        public required long Id { get; set; }

        public required string NoteText { get; set; }

        public required UserDTO User { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime? UpdatedAt { get; set; }

        public required int LikeCount { get; set; }

        public required int ReplyCount { get; set; }

        public bool IsLiked { get; set; } = false;

        public required VerseSimpleDTO Verse { get; set; }
    }
    public static class NoteDTOExtension
    {

        public static NoteDTO ToNoteDTO(this Note note, User UserRequested)
        {
            return new NoteDTO
            {
                Id = note.Id,
                NoteText = note.Text,
                User = note.User.ToUserDTO(),
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                LikeCount = note.Likes?.Count ?? 0,
                ReplyCount = note.Comments?.Count ?? 0,
                IsLiked = note.Likes?.Any(ln => ln.Like != null && ln.Like.UserId == UserRequested.Id) ?? default
            };
        }

        public static NoteDTOExtended ToNoteDTOExtended(this Note note, User UserRequested)
        {
            return new NoteDTOExtended
            {
                Id = note.Id,
                NoteText = note.Text,
                User = note.User.ToUserDTO(),
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt,
                LikeCount = note.Likes?.Count ?? 0,
                ReplyCount = note.Comments?.Count ?? 0,
                IsLiked = note.Likes?.Any(ln => ln.Like != null && ln.Like.UserId == UserRequested.Id) ?? default,
                Verse = note.Verse.ToVerseSimpleDTO()
            };
        }
    }
}