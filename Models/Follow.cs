using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using writings_backend_dotnet.Models;

public class Follow
{
    [Key]
    public long Id { get; set; }

    [Required]
    public Guid FollowerId { get; set; }

    [Required]
    public Guid FollowedId { get; set; }

    [Required]
    public FollowStatus Status { get; set; }

    [Required]
    public DateTime OccurredAt { get; set; }

    [ForeignKey("FollowerId")]
    public User? Follower { get; set; }

    [ForeignKey("FollowedId")]
    public User? Followed { get; set; }
}
