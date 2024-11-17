using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    [Table("note")]
    public class Note
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("user_id", TypeName = DBTypeUUID), ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [Required, Column("text", TypeName = "text")]
        public string Text { get; set; } = null!;

        [Required, Column("verse_id", TypeName = DBType32bitInteger), ForeignKey("Verse")]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;

        [Column("created_at", TypeName = DBTypeDateTime)]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = DBTypeDateTime)]
        public DateTime? UpdatedAt { get; set; }

        public virtual List<CommentNote>? Comments { get; set; }

        public virtual List<LikeNote>? Likes { get; set; }

    }
}
