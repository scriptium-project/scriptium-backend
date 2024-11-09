using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using writings_backend_dotnet.Models;
using writings_backend_dotnet.Models.Util;

public class FreezeR
{
    [Key]
    public long Id { get; set; }

    [Required]
    public FreezeStatus Status { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public DateTime ProceedAt { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
