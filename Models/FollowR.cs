using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using scriptium_backend_dotnet.Models.Util;

namespace scriptium_backend_dotnet.Models
{
    public class FollowR
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid FollowerId { get; set; }

        [Required]
        public Guid FollowedId { get; set; }

        [Required]
        public FollowRStatus Status { get; set; }

        [Required]
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("FollowerId")]
        public virtual User Follower { get; set; } = null!;

        [ForeignKey("FollowedId")]
        public virtual User Followed { get; set; } = null!;
    }
}