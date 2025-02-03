using FluentValidation;
using SixLabors.ImageSharp;

namespace scriptium_backend_dotnet.Controllers.Validation
{
    public class UpdateProfileModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Biography { get; set; }
        public string? Gender { get; set; }
        public byte? LanguageId { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class UpdateProfileValidator : AbstractValidator<UpdateProfileModel>
    {
        private readonly long _maxFileSize = 8 * 1024 * 1024; //8MB
        private readonly long _requiredHeight = 1024;
        private readonly long _requiredWidth = 1024;


        public UpdateProfileValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(16).WithMessage("Name cannot exceed 16 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Surname)
                .MaximumLength(16).WithMessage("Surname cannot exceed 16 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Surname));

            RuleFor(x => x.Username)
                .MinimumLength(5)
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

            RuleFor(r => r.Image)
                                .Must(IsAllowedExtension).WithMessage("Only JPEG or JPG files are allowed.")
                                .Must(File => File != null && File.Length <= _maxFileSize).WithMessage($"Image size must be less than {_maxFileSize / (1024 * 1024)} MB.")
                                .Must(IsValidImage).WithMessage($"Image must be {_requiredWidth}x{_requiredHeight} pixels and square.");

        }

        private bool IsAllowedExtension(IFormFile? File)
        {
            if (File == null)
                return false;

            string[] allowedExtensions = [".jpg", ".jpeg"];
            string extension = Path.GetExtension(File.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        private bool IsValidImage(IFormFile? File)
        {

            if (File == null)
                return false;

            try
            {
                using var stream = File.OpenReadStream();
                using var image = Image.Load(stream);

                return image.Width == image.Height && image.Width == _requiredWidth; //&& image.Height == _requiredHeight; Should be square.
            }
            catch
            {
                return false;
            }
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

    public class PasswordModel
    {
        public required string Password { get; set; }
    }

    public class PasswordModelValidator : AbstractValidator<PasswordModel>
    {
        public PasswordModelValidator()
        {

            RuleFor(x => x.Password).AuthenticationPasswordRules();

        }
    }

}