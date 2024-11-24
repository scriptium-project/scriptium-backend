using FluentValidation;
using writings_backend_dotnet.Models.Util;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class CommentLikeProcessModel
    {
        public required long CommentId { get; set; }
        public required LikeableEntityType EntityType { get; set; }
    }

    public class CommentLikeProcessModelValidator : AbstractValidator<CommentLikeProcessModel>
    {
        public CommentLikeProcessModelValidator()
        {
            RuleFor(r => r.CommentId)
            .GreaterThan(0).WithMessage("Variable CommentId must be greater than 0.");
        }
    }

    public class EntityCommentCreateModel
    {
        public required long EntityId { get; set; }
        public required string CommentText { get; set; }
        public required long? ParentCommentId { get; set; }
    }

    public class EntityCommentCreateModelValidator : AbstractValidator<EntityCommentCreateModel>
    {
        public EntityCommentCreateModelValidator()
        {
            RuleFor(r => r.EntityId).NotEmpty()
            .GreaterThan(0).WithMessage("Variable EntityId must be greater than 0.");

            RuleFor(r => r.CommentText)
            .MaximumLength(250).WithMessage("CommentText cannot exceed 250 characters.");
        }
    }


    public class CommentUpdateModel
    {
        public required string NewCommentText { get; set; }
        public required long CommentId { get; set; }
    }

    public class CommentUpdateModelValidator : AbstractValidator<CommentUpdateModel>
    {
        public CommentUpdateModelValidator()
        {
            RuleFor(r => r.CommentId)
            .GreaterThan(0).WithMessage("Variable CommentId must be greater than 0.");

            RuleFor(r => r.NewCommentText)
            .MaximumLength(250).WithMessage("Comment text cannot exceed 250 characters.");
        }
    }

    public class CommentDeleteModel
    {
        public required long CommentId { get; set; }
    }

    public class CommentDeleteModelValidator : AbstractValidator<CommentDeleteModel>
    {
        public CommentDeleteModelValidator()
        {
            RuleFor(r => r.CommentId)
            .GreaterThan(0).WithMessage("Variable CommentId must be greater than 0.");
        }
    }
}