using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("comment_note")]
    public class CommentNote
    {
        [Key, Column("comment_id", TypeName = "bigint")]
        public long CommentId { get; set; }

        [Required, Column("note_id", TypeName = "integer")]
        public int NoteId { get; set; }

        public Comment? Comment { get; set; }

        public Note? Note { get; set; }
    }

}
