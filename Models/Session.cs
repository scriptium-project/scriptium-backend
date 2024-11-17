using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("session")]
    public class Session
    {
        [Key]
        public string Key { get; set; } = null!;
    
        [Required]
        public string Value { get; set; } = null!; 
    
        public DateTime? ExpiresAt { get; set; }
    }

}
