using Microsoft.AspNetCore.Identity;
using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    // public class GetCommentDTO
    // {
    //     public required string Text { get; set; }
    //     public CommentParentCommentDTO? ParentComment { get; set; }
    //     public NoteDTO? Note { get; set; }
    //     public VerseDTO? Verse { get; set; }
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
    //             commentDTO.Verse = comment.CommentVerse.Verse.ToVerseDTO();
    //         if (comment.CommentNote != null)
    //             commentDTO.Note = comment.CommentNote.Note.ToNoteDTO(isFollowing);

    //         return commentDTO;
    //     }
    // }

    public class CommentExtendedParentCommentDTO
    {
        public required long Id { get; set; }
        public required string Text { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime? UpdatedAt { get; set; }
        public required long LikeCount { get; set; }
        public required long ReplyCount { get; set; }
        public bool IsLiked { get; set; } = false;
        public required VerseSimpleDTO Verse { get; set; }
        public required ParentCommentDTO? ParentComment { get; set; } = null;

    }

    public class ParentCommentDTO
    {
        public required long Id { get; set; }
        public required ParentCommentOwnerUserDTO? User { get; set; }
        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ParentCommentOwnerUserDTO
    {
        public required string Username { get; set; }
        public byte[]? Image { get; set; }
        public required string Name { get; set; }
        public string? Surname { get; set; }
    }

    public static class CommentParentUserDTOExtension
    {

        public static CommentExtendedParentCommentDTO ToCommentExtendedParentCommentDTO(this Comment Comment, bool hasPermissionToSeeParentCommentOwnerInformation)
        {

            if (Comment.CommentVerse == null) throw new ArgumentNullException();

            return new CommentExtendedParentCommentDTO
            {
                Id = Comment.Id,
                Text = Comment.Text,
                CreatedAt = Comment.CreatedAt,
                UpdatedAt = Comment.UpdatedAt ?? default,
                LikeCount = Comment.LikeCount,
                ReplyCount = Comment.ReplyCount,
                Verse = Comment.CommentVerse.Verse.ToVerseSimpleDTO(),
                ParentComment = Comment.ParentComment != null ? new ParentCommentDTO
                {
                    Id = Comment.ParentComment.Id,
                    Text = Comment.ParentComment.Text,
                    CreatedAt = Comment.ParentComment.CreatedAt,
                    UpdatedAt = Comment.ParentComment.UpdatedAt,
                    User = hasPermissionToSeeParentCommentOwnerInformation ? new ParentCommentOwnerUserDTO
                    {
                        Username = Comment.ParentComment.User.UserName,
                        Image = Comment.ParentComment.User.Image,
                        Name = Comment.ParentComment.User.Name,
                        Surname = Comment.ParentComment.User.Surname
                    } : null,

                } : null

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
        public bool IsLiked { get; set; } = false;
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
                ReplyCount = comment.ReplyCount,

            };
        }

        public static GetCommentDTO ToGetCommentDTO(this Comment comment, User UserRequested)
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
                ReplyCount = comment.ReplyCount,
                IsLiked = comment.LikeComments?.Any(c => c.Like.UserId == UserRequested.Id) ?? false

            };
        }
    }


    public class CommentDTOExtended
    {
        public long Id { get; set; }
        public required UserDTO User { get; set; }
        public required string Text { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? ParentCommentId { get; set; }
        public required long LikeCount { get; set; }
        public required long ReplyCount { get; set; }
        public bool IsLiked { get; set; } = false;
        public required VerseSimpleDTO Verse { get; set; }
    }

    public static class CommentDTOExtendedExtension
    {

        public static CommentDTOExtended ToCommentDTOExtended(this Comment comment, User UserRequested)
        {


            Verse? verse = comment.CommentVerse?.Verse;

            if (verse == null) throw new ArgumentNullException();


            return new CommentDTOExtended
            {
                Id = comment.Id,
                User = comment.User.ToUserDTO(),
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                ParentCommentId = comment.ParentCommentId,
                LikeCount = comment.LikeCount,
                ReplyCount = comment.ReplyCount,
                IsLiked = comment.LikeComments?.Any(c => c.Like.UserId == UserRequested.Id) ?? false,
                Verse = new VerseSimpleDTO
                {
                    Id = verse.Id,
                    Number = verse.Number,
                    Text = verse.Text,
                    TextWithoutVowel = verse.TextWithoutVowel,
                    TextSimplified = verse.TextSimplified,
                    Chapter = verse.Chapter.ToChapterDTO()

                }

            };

        }
    }
}



