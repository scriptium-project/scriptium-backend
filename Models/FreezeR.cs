using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet
{


    public class FreezeR
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required FreezeStatus Status { get; set; }

        [Required]
        public required Guid UserId { get; set; }

        [Required]
        public DateTime ProceedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}