using FluentValidation;

namespace scriptium_backend_dotnet.Controllers.Validation
{
    public class GetFollowerModel
    {
        public string Type { get; set; } = null!;
    }
    public class TypeValidator : AbstractValidator<GetFollowerModel>
    {
        public TypeValidator()
        {
            RuleFor(Type => Type)
                .NotEmpty()
                .WithMessage("Type cannot be empty.")
                .Must(Model => Model.Type == "Follower" || Model.Type == "Pending")
                .WithMessage("Type must be either 'follower' or 'pending'.");
        }
    }


}