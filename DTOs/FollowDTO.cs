using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class FollowUserDTO
    {
        public required string Username { get; set; }
        public required string? Image { get; set; }
        public required string Name { get; set; }
        public required string? Surname { get; set; }
        public required DateTime OccurredAt { get; set; }
        public required bool IsFrozen { get; set; }
    }
    public static class FollowUserDTOExtensions
    {
        public static FollowUserDTO ToFollowerUserDTO(this Follow Follow)
        {
            return new FollowUserDTO
            {
                Username = Follow.Follower.UserName,
                Image = Follow.Follower.Image != null && !Follow.Follower.IsFrozen.HasValue ? Convert.ToBase64String(Follow.Follower.Image) : null!,
                Name = Follow.Follower.Name,
                Surname = Follow.Follower.Surname,
                OccurredAt = Follow.OccurredAt,
                IsFrozen = Follow.Follower.IsFrozen.HasValue
            };
        }

        public static FollowUserDTO ToFollowingUserDTO(this Follow Follow)
        {
            return new FollowUserDTO
            {
                Username = Follow.Followed.UserName,
                Image = Follow.Follower.Image != null && !Follow.Follower.IsFrozen.HasValue ? Convert.ToBase64String(Follow.Followed.Image) : null!,
                Name = Follow.Followed.Name,
                Surname = Follow.Followed.Surname,
                OccurredAt = Follow.OccurredAt,
                IsFrozen = Follow.Followed.IsFrozen.HasValue
            };
        }
    }
}