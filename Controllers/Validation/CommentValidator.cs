using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class GetCommentsFromNoteModel
    {
        public required long NoteId { get; set; }
    }

    public class GetCommentsFromNoteModelValidator : AbstractValidator<GetCommentsFromNoteModel>
    {
        public GetCommentsFromNoteModelValidator()
        {
            RuleFor(r => r.NoteId).GreaterThan(0)
            .WithMessage("Variable NoteId must be greater than 0.");


        }
    }

    public class GetCommentsFromVerseModel
    {
        public required long CommentId { get; set; }
    }

    public class GetCommentsFromVerseModelValidator : AbstractValidator<GetCommentsFromVerseModel>
    {
        public GetCommentsFromVerseModelValidator()
        {
            RuleFor(r => r.CommentId)
            .GreaterThan(0).WithMessage("Variable CommentId must be greater than 0.");
        }
    }

    public class EntityCommentCreateModel
    {
        public required long EntityId { get; set; }
        public required string CommentText { get; set; }
        public required long ParentCommentId { get; set; }
    }

    public class EntityCommentCreateModelValidator : AbstractValidator<EntityCommentCreateModel>
    {
        public EntityCommentCreateModelValidator()
        {
            RuleFor(r => r.EntityId)
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