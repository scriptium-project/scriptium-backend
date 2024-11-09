using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Role
    {
        [Key, Column("id")]
        public short Id { get; set; }

        [Required, Column("role_name")]
        public required string RoleName { get; set; }

        public string? Description { get; set; }

        public List<User>? Users { get; set; }
    }
}