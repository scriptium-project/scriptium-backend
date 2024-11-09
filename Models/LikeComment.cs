using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{
    public class LikeComment
    {
        [Key]
        public long LikeId { get; set; }

        [Required]
        public long CommentId { get; set; }

        public Like? Like { get; set; }

        public Comment? Comment { get; set; }
    }
}