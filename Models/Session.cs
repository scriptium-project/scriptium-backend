using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("session")]
    public class Session
    {
        [Key, Column("id", TypeName = "varchar(100)"), Required]
        public string Id { get; set; } = null!;

        [ForeignKey("User"), Column("user_id", TypeName = "uuid")]
        public Guid? UserId { get; set; }

        public User? User { get; set; }

        [Column("expires_at", TypeName = "timestamp")]
        public DateTime? ExpiresAt { get; set; }

        [Required, Column("session", TypeName = "jsonb")]
        public string SessionData { get; set; } = null!;

    }
}
