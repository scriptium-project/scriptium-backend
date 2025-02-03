using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class BlockDTO
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public string? Surname { get; set; }
        public required DateTime BlockedAt { get; set; }
        public string? Image { get; set; }
        public string? Reason { get; set; }
    }
    public static class BlockExtension
    {
        public static BlockDTO ToBlockDTO(this Block Block)
        {
            User BlockUser = Block.Blocked;
            return new BlockDTO
            {
                Id = BlockUser.Id,
                Username = BlockUser.UserName!,
                Name = BlockUser.Name,
                Surname = BlockUser.Surname,
                Image = BlockUser.Image != null ? Convert.ToBase64String(BlockUser.Image) : null!,
                Reason = Block.Reason,
                BlockedAt = Block.BlockedAt,
            };

        }


    }
}