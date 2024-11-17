using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{
    public class LikeComment
    {
        [Key]
        public long LikeId { get; set; }

        [Required]
        public long CommentId { get; set; }

        public virtual Like Like { get; set; } = null!;

        public virtual Comment Comment { get; set; } = null!;
    }
}