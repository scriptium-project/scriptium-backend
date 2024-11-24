using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("comment_verse")]
    public class CommentVerse
    {
        [Key, Column("comment_id", TypeName = Utility.DBType64bitInteger)]
        public long CommentId { get; set; }

        [Required, Column("verse_id", TypeName = Utility.DBType32bitInteger)]
        public int VerseId { get; set; }

        public virtual Comment Comment { get; set; } = null!;

        public virtual Verse Verse { get; set; } = null!;
    }

}
