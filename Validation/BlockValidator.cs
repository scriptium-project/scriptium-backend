
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class BlockModel
    {
        public required string UserName { get; set; }
        public string? Reason { get; set; }
    }
    public class BlockModelValidator : AbstractValidator<BlockModel>
    {
        public BlockModelValidator()
        {
            RuleFor(r => r.UserName)
                         .NotEmpty().WithMessage("Username is required.")
                         .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.");

            RuleFor(r => r.Reason)
            .MaximumLength(100).WithMessage("Reason cannot exceed 100 characters.");

        }
    }
}