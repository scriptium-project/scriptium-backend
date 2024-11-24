using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("session")]
    public class Session
    {
        [Key]
        public string Key { get; set; } = null!;

        // [Required, Column(TypeName = Utility.DBTypeVARBINARYMAX)] //TODO:Binary Data
        // public byte[] Data { get; set; } = null!; 

        public required Guid UserId { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }

}
