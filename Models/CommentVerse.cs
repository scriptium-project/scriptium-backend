using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("comment_verse")]
    public class CommentVerse
    {
        [Key, Column("comment_id", TypeName = "bigint")]
        public long CommentId { get; set; }

        [Required, Column("verse_id", TypeName = "integer")]
        public int VerseId { get; set; }

        public Comment? Comment { get; set; }

        public Verse? Verse { get; set; }
    }

}
