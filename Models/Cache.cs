using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace writings_backend_dotnet.Models
{
    public class Cache
    {
        [Key, Column(TypeName = "bigint")]
        public long Id { get; set; }

        [Required, MaxLength(126)]
        public required string Key { get; set; }

        [Column(TypeName = "jsonb")]
        public required string Data { get; set; }

        [Required, Column(TypeName = "timestamp")]
        public DateTime ExpirationDate { get; set; } = DateTime.Now;
    }
}
