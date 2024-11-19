using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    [Table("user")]
    public class User : IdentityUser<Guid>
    {

        [Required, MaxLength(30)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(30)]
        public string Surname { get; set; } = null!;

        public byte[] Image { get; set; } = null!; //TODO: Will be implemented

        [MaxLength(1)]
        public string? Gender { get; set; }

        [MaxLength(200)]
        public string? Biography { get; set; }

        public DateTime? EmailVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastActive { get; set; }

        public DateTime? IsFrozen { get; set; }

        public DateTime? IsPrivate { get; set; }

        [Column("preferred_languageId", TypeName = DBType8bitInteger)]
        public byte PreferredLanguageId { get; set; } = 1;

        [ForeignKey("PreferredLanguageId")]
        public virtual Language PreferredLanguage { get; set; } = null!;

        public virtual List<Session>? Sessions { get; set; }

        public virtual List<Collection>? Collections { get; set; }

        public virtual List<Note>? Notes { get; set; }

        public virtual List<Comment>? Comments { get; set; }

        public virtual List<Follow>? Followers { get; set; }

        public virtual List<Follow>? Following { get; set; }

        public virtual List<FollowR>? FollowerRs { get; set; }

        public virtual List<FollowR>? FollowRing { get; set; }

        public virtual List<Block>? BlockedUsers { get; set; }

        public virtual List<Block>? BlockedByUsers { get; set; }

        public virtual List<FreezeR>? FreezeRecords { get; set; }

        public virtual List<Like>? Likes { get; set; }

        public virtual List<Notification>? NotificationsReceived { get; set; }

        public virtual List<Notification>? NotificationsSent { get; set; }

        public virtual List<Suggestion>? Suggestions { get; set; }

    }
}
