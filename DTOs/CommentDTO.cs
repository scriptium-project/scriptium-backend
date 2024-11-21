using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class GetCommentDTO
    {
        public required string Text { get; set; }
        public CommentParentCommentDTO? ParentComment { get; set; }
        public NoteDTO? Note { get; set; }
        public VerseSimpleDTO? Verse { get; set; }
        public int LikeCount { get; set; }

    }
    public static class GetCommentExtension
    {

        public static GetCommentDTO ToGetCommentDTO(this Comment comment, bool isFollowing)
        {

            GetCommentDTO commentDTO = new()
            {
                Text = comment.Text,
                ParentComment = comment.ParentComment?.ToCommentParentUserDTO(isFollowing),
            };
            if (comment.CommentVerse != null)
                commentDTO.Verse = comment.CommentVerse.Verse.ToVerseSimpleDTO();
            if (comment.CommentNote != null)
                commentDTO.Note = comment.CommentNote.Note.ToNoteDTO(isFollowing);

            return commentDTO;
        }
    }

    public class CommentParentCommentDTO
    {
        public UserDTO? User { get; set; }
        public DateTime? CreatedAt { get; set; }
        public required string ParentCommentText;

    }

    public static class CommentParentUserDTOExtension
    {


        public static CommentParentCommentDTO ToCommentParentUserDTO(this Comment parentComment, bool isFollowing)
        {
            return new CommentParentCommentDTO
            {
                ParentCommentText = parentComment.Text,
                User = isFollowing ? parentComment.User.ToUserDTO() : null,
                CreatedAt = isFollowing ? parentComment.CreatedAt : null,
            };

        }
    }
}



