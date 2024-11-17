using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Cache
    {
        [Key, Column("id",TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, MaxLength(126)]
        public required string Key { get; set; }

        [Required, Column("data", TypeName = DBTypeVARCHARMAX)]
        public required string Data { get; set; }

        [Required, Column(TypeName = DBTypeDateTime)]
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;

        public virtual List<CacheR>? CacheRs { get; set; }
    }
}
