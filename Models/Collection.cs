using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;
namespace writings_backend_dotnet.Models
{
    [Table("collection")]
    public class Collection
    {
        [Key, Column("id", TypeName = DBTypeUUID)]
        public Guid Id { get; set; }

        [Required, Column("name", TypeName = DBTypeVARCHAR100), MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(250), Column("description", TypeName = DBTypeVARCHAR250)]
        public string? Description { get; set; }

        [Required, ForeignKey("User"), Column("user_id", TypeName = DBTypeUUID)]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [Column("created_at", TypeName = DBTypeDateTime)]
        public DateTime CreatedAt { get; set; }

        public virtual List<CollectionVerse>? Verses { get; set; }

    }
}
