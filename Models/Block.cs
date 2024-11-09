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

        public DateTime? BlockedAt { get; set; }

        [MaxLength(100)]
        public string? Reason { get; set; }

        [ForeignKey("BlockerId")]
        public User? Blocker { get; set; }

        [ForeignKey("BlockedId")]
        public User? Blocked { get; set; }
    }
}