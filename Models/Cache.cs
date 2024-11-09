using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace writings_backend_dotnet.Models
{
    public class Cache
    {
        [Key, Column(TypeName = "bigint")]
        public long Id { get; set; }

        [Required]
        [MaxLength(126)]
        public required string Key { get; set; }

        [Column(TypeName = "jsonb")]
        public JsonDocument Data { get; set; } = null!;
    }

}