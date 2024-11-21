using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class UserNameModel
    {
        public required string UserName { get; set; }
    }
    public class FollowModelValidator : AbstractValidator<UserNameModel>
    {
        public FollowModelValidator()
        {
            RuleFor(r => r.UserName)
                         .NotEmpty().WithMessage("Username is required.")
                         .MaximumLength(16).WithMessage("Username cannot exceed 16 characters.");

        }
    }
}