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

        public virtual User User { get; set; } = null!;

        public virtual LikeComment? LikeComment { get; set; }

        public virtual LikeNote? LikeNote { get; set; }
    }

}