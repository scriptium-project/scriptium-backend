using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("comment")]
    public class Comment
    {
        [Key, Column("id", TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, ForeignKey("User"), Column("user_id", TypeName = "uuid")]
        public Guid UserId { get; set; }

        public User? User { get; set; }

        [Required, MaxLength(500), Column("text", TypeName = "varchar(500)")]
        public string Text { get; set; } = null!;

        [Column("created_at", TypeName = "timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ParentComment"), Column("parent_comment_id", TypeName = "bigint")]
        public long? ParentCommentId { get; set; }

        public Comment? ParentComment { get; set; }

        public List<Comment>? Replies { get; set; }

        public long? CommentVerseId { get; set; } = null!;

        public CommentVerse? CommentVerse { get; set; }

        public long? CommentNoteId { get; set; } = null!;

        public CommentNote? CommentNote { get; set; }

        public List<LikeComment>? LikeComments { get; set; }


    }
}
