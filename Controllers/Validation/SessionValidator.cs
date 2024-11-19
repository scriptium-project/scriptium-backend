using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class UpdateProfileModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Biography { get; set; }
        public string? Gender { get; set; }
        public byte? LanguageId { get; set; }
    }

    public class UpdateProfileValidator : AbstractValidator<UpdateProfileModel>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(16).WithMessage("Name cannot exceed 16 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Surname)
                .MaximumLength(16).WithMessage("Surname cannot exceed 16 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Surname));

            RuleFor(x => x.Username)
                .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.")
                .Matches("^[a-zA-Z0-9._]*$").WithMessage("Username can only contain letters, numbers, dots, and underscores.")
                .When(x => !string.IsNullOrWhiteSpace(x.Username));

            RuleFor(x => x.Biography)
                .MaximumLength(200).WithMessage("Biography cannot exceed 200 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Biography));

            RuleFor(r => r.Gender)
              .MaximumLength(1).WithMessage("Gender must be a single character.")
              .Must(g => string.IsNullOrEmpty(g) || g == "M" || g == "F" || g == "O")
              .WithMessage("Invalid gender. Allowed values are 'M', 'F', or 'O'.");

            RuleFor(x => x.LanguageId)
                .GreaterThanOrEqualTo((byte)1).WithMessage("Language ID must be a valid positive number.")
                .When(x => x.LanguageId.HasValue);
        }
    }
    public class ChangePasswordModel
    {
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }


}