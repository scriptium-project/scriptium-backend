using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    [Table("comment_verse")]
    public class CommentVerse
    {
        [Key, Column("comment_id", TypeName = DBType64bitInteger)]
        public long CommentId { get; set; }

        [Required, Column("verse_id", TypeName = DBType32bitInteger)]
        public int VerseId { get; set; }

        public virtual Comment Comment { get; set; } = null!;

        public virtual Verse Verse { get; set; } = null!;
    }

}
