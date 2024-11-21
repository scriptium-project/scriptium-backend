using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{

    public class CollectionCreateModel
    {
        public required string CollectionName { get; set; }
        public string? Description { get; set; }
    }
    public class CollectionCreateModelValidator : AbstractValidator<CollectionCreateModel>
    {
        public CollectionCreateModelValidator()
        {
            RuleFor(r => r.CollectionName)
                         .NotEmpty().MinimumLength(1).WithMessage("Collection name cannot be empty or null.")
                         .MaximumLength(100).WithMessage("Collection name cannot exceed 100 characters.");

            RuleFor(r => r.Description)
            .MaximumLength(250).WithMessage("Collection description cannot exceed 250 characters.");

        }
    }

    public class CollectionUpdateModel
    {
        public required string OldCollectionName { get; set; }
        public required string NewCollectionName { get; set; }
        public string? NewDescription { get; set; }
    }
    public class CollectionUpdateModelValidator : AbstractValidator<CollectionUpdateModel>
    {
        public CollectionUpdateModelValidator()
        {

            RuleFor(r => r.OldCollectionName)
                         .NotEmpty().MinimumLength(1).WithMessage("Old collection name cannot be empty or null.")
                         .MaximumLength(100).WithMessage("Old collection name cannot exceed 100 characters.");


            RuleFor(r => r.NewCollectionName)
                         .NotEmpty().MinimumLength(1).WithMessage("Collection name cannot be empty or null.")
                         .MaximumLength(100).WithMessage("Collection name cannot exceed 100 characters.");

            RuleFor(r => r.NewDescription)
            .MaximumLength(250).WithMessage("Collection new description cannot exceed 250 characters.");

        }
    }

    public class CollectionDeleteModel
    {
        public required string CollectionName { get; set; }
    }
    public class CollectionDeleteModelValidator : AbstractValidator<CollectionDeleteModel>
    {
        public CollectionDeleteModelValidator()
        {

            RuleFor(r => r.CollectionName)
                                   .NotEmpty().MinimumLength(1).WithMessage("Collection name cannot be empty or null.")
                                   .MaximumLength(100).WithMessage("Collection name cannot exceed 100 characters.");

        }
    }
}