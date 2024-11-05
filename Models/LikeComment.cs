using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
public class LikeComment
{
    [Key]
    public long LikeId { get; set; }
    
    [Required]
    public long CommentId { get; set; }

    public required Like Like { get; set; }

    public required Comment Comment { get; set; }
}
}