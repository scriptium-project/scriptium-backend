using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    [Table("comment")]
    public class Comment
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, ForeignKey("User"), Column("user_id", TypeName = DBTypeUUID)]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [Required, MaxLength(250), Column("text", TypeName = DBTypeVARCHAR250)]
        public string Text { get; set; } = null!;

        [Column("created_at", TypeName = DBTypeDateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at", TypeName = DBTypeDateTime)]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("ParentComment"), Column("parent_comment_id", TypeName = DBType64bitInteger)]
        public long? ParentCommentId { get; set; }

        public virtual Comment? ParentComment { get; set; } = null!;

        public virtual List<Comment>? Replies { get; set; }

        public long? CommentVerseId { get; set; } = null!;

        public virtual CommentVerse? CommentVerse { get; set; }

        public long? CommentNoteId { get; set; } = null!;

        public virtual CommentNote? CommentNote { get; set; }

        public virtual List<LikeComment>? LikeComments { get; set; }


    }
}
