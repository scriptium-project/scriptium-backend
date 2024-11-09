using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("user")]
    public class User
    {
        [Key, Column("id", TypeName = "uuid"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, MaxLength(24)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Surname { get; set; } = null!;

        [MaxLength(1)]
        public string? Gender { get; set; }

        [MaxLength(200)]
        public string? Biography { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; } = null!;

        public DateTime? EmailVerified { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? LastActive { get; set; }

        public DateTime? IsFrozen { get; set; }

        public DateTime? IsPrivate { get; set; }

        public short? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        [Column("preferred_languageId", TypeName = "smallint")]
        public short PreferredLanguageId { get; set; } = 1;

        [ForeignKey("PreferredLanguageId")]
        public Language? PreferredLanguage { get; set; }

        public List<Session>? Sessions { get; set; }

        public List<Collection>? Collections { get; set; }

        public List<Note>? Notes { get; set; }

        public List<Comment>? Comments { get; set; }

        public List<Follow>? Followers { get; set; }

        public List<Follow>? Following { get; set; }

        public List<Block>? BlockedUsers { get; set; }

        public List<Block>? BlockedByUsers { get; set; }

        public List<FreezeR>? FreezeRecords { get; set; }

        public List<Like>? Likes { get; set; }

        public List<Notification>? NotificationsReceived { get; set; }

        public List<Notification>? NotificationsSent { get; set; }


    }
}
