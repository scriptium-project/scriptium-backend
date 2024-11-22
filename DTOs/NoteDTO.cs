using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class NoteDTO
    {
        public required long Id { get; set; }
        public required string NoteText { get; set; }
        public required VerseSimpleDTO Verse { get; set; }
        public UserDTO? User { get; set; }
    }
    public static class NoteDTOExtension
    {
        public static NoteDTO ToNoteDTO(this Note note, bool isFollowing)
        {
            return new NoteDTO
            {
                Id = note.Id,
                NoteText = note.Text,
                Verse = note.Verse.ToVerseSimpleDTO(),
                User = isFollowing ? note.User.ToUserDTO() : null,
            };
        }

        public static NoteDTO ToNoteDTO(this Note note)
        {
            return new NoteDTO
            {
                Id = note.Id,
                NoteText = note.Text,
                Verse = note.Verse.ToVerseSimpleDTO(),
                User = note.User.ToUserDTO()
            };
        }
    }
}