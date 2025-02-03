using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.DTOs
{
    public class UserDTO
    {
        public required Guid Id { get; set; }

        public required string Username { get; set; }

        public byte[]? Image { get; set; }

        public required string Name { get; set; }
        public string? Surname { get; set; }
    }

    public static class UserDTOExtension
    {
        public static UserDTO ToUserDTO(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Image = user.Image
            };

        }
    }
}