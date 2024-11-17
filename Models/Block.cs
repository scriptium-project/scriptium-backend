using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace writings_backend_dotnet.Models
{
    public class Block
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid BlockerId { get; set; }

        [Required]
        public Guid BlockedId { get; set; }

        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Reason { get; set; }

        [ForeignKey("BlockerId")]
        public virtual User Blocker { get; set; } = null!;

        [ForeignKey("BlockedId")]
        public virtual User Blocked { get; set; } = null!;
    }
}