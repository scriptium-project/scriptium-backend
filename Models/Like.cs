using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{
    public class Like
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }

        public LikeComment? LikeComment { get; set; }

        public LikeNote? LikeNote { get; set; }
    }

}