using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
public class CacheR
{
    [Key, Column("id", TypeName = DBType64bitInteger)]
    public long Id { get; set; } 
    [Required, Column("cache_id", TypeName = DBType64bitInteger)]
    public long CacheId { get; set; } 

    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

    public virtual Cache Cache { get; set; } = null!;
}

}