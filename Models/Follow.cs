using System;
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
    public required User Follower { get; set; }

    [ForeignKey("FollowedId")]
    public required User Followed { get; set; }
}
