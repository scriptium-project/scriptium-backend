using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("note")]
    public class Note
    {
        [Key]
        [Column("id", TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column("user_id", TypeName = "uuid"), ForeignKey("User")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [Required,Column("text", TypeName = "text")]
        public string Text { get; set; } = null!;

        [Required, Column("verse_id", TypeName = "integer"), ForeignKey("Verse")]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;

        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

        public virtual List<CommentNote> Comments { get; set; } = [];

        public virtual List<LikeNote> LikeNotes { get; set; } = [];

    }
}
