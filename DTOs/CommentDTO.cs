using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    // public class GetCommentDTO
    // {
    //     public required string Text { get; set; }
    //     public CommentParentCommentDTO? ParentComment { get; set; }
    //     public NoteDTO? Note { get; set; }
    //     public VerseSimpleDTO? Verse { get; set; }
    //     public int LikeCount { get; set; }

    // }
    // public static class GetCommentExtension
    // {

    //     public static GetCommentDTO ToGetCommentDTO(this Comment comment, bool isFollowing)
    //     {

    //         GetCommentDTO commentDTO = new()
    //         {
    //             Text = comment.Text,
    //             ParentComment = comment.ParentComment?.ToCommentParentUserDTO(isFollowing),
    //         };
    //         if (comment.CommentVerse != null)
    //             commentDTO.Verse = comment.CommentVerse.Verse.ToVerseSimpleDTO();
    //         if (comment.CommentNote != null)
    //             commentDTO.Note = comment.CommentNote.Note.ToNoteDTO(isFollowing);

    //         return commentDTO;
    //     }
    // }

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

    public class GetCommentDTO
    {
        public long Id { get; set; }
        public required UserDTO User { get; set; }
        public required string Text { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? ParentCommentId { get; set; }
        public required long LikeCount { get; set; }
        public required long ReplyCount { get; set; }
    }


    public static class GetCommentDTOExtension
    {


        public static GetCommentDTO ToGetCommentDTO(this Comment comment)
        {
            return new GetCommentDTO
            {
                Id = comment.Id,
                User = comment.User.ToUserDTO(),
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                ParentCommentId = comment.ParentCommentId,
                LikeCount = comment.LikeCount,
                ReplyCount = comment.ReplyCount
            };

        }


    }

}



