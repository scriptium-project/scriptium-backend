using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
public class LikeNote
{
    [Key]
    public long LikeId { get; set; }

    [Required]
    public int NoteId { get; set; }

    public required Like Like { get; set; }

    public required Note Note { get; set; }
}
}