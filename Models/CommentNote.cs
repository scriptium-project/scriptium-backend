using System;
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

        public virtual Comment Comment { get; set; } = null!;

        public virtual Note Note { get; set; } = null!;
    }

}
