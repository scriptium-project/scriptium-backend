using System;
using System.Threading.Tasks;
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class RegisterModel
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Surname { get; set; }

        public string? Gender { get; set; }
    }

    public class AuthValidator : AbstractValidator<RegisterModel>
    {
        public AuthValidator()
        {
            RuleFor(r => r.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.")
                .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(16).WithMessage("Name cannot exceed 16 characters.");

            RuleFor(r => r.Surname)
                .MaximumLength(16).WithMessage("Surname cannot exceed 16 characters.");

            RuleFor(r => r.Gender)
                .MaximumLength(1).WithMessage("Gender must be a single character.")
                .Must(g => string.IsNullOrEmpty(g) || g == "M" || g == "F" || g == "O")
                .WithMessage("Invalid gender. Allowed values are 'M', 'F', or 'O'.");
        }

        public class LoginModel
        {
            public string Username { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginValidator : AbstractValidator<LoginModel>
        {
            public LoginValidator()
            {
                RuleFor(l => l.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.");

                RuleFor(l => l.Password)
                    .NotEmpty().WithMessage("Password is required.")
                    .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.");
            }
        }
    }
}
