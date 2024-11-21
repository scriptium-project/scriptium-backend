using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.DTOs
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public byte[]? Image { get; set; }
    }

    public static class UserDTOExtension
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO
            {
                Username = user.UserName,
                Image = user.Image
            };

        }
    }
}