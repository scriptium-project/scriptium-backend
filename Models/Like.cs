using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
    public class Like
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public required User User { get; set; }

        public LikeComment LikeComment { get; set; } = null!;
        
        public LikeNote LikeNote { get; set; } = null!;
    }

}