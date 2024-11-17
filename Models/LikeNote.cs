using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{
    public class LikeNote
    {
        [Key]
        public long LikeId { get; set; }

        [Required]
        public long NoteId { get; set; }

        public virtual Like Like { get; set; } = null!;

        public virtual Note Note { get; set; } = null!;
    }
}