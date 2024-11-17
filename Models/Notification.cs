using System.ComponentModel.DataAnnotations;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Models
{
    public class Notification
    {
        public long Id { get; set; }

        [Required]
        public Guid RecipientId { get; set; }

        [Required]
        public Guid ActorId { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public EntityType? EntityType { get; set; } 

        public Guid? EntityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsRead { get; set; }

        public virtual User Recipient { get; set; } = null!;

        public virtual User Actor { get; set; } = null!;
    }

}