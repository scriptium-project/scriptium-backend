using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class LikeNoteModel
    {
        public required long NoteId { get; set; }
    }

    public class LikeNoteModelValidator : AbstractValidator<LikeNoteModel>
    {
        public LikeNoteModelValidator()
        {
            RuleFor(r => r.NoteId)
            .GreaterThan(0).WithMessage("Variable NoteId must be greater than 0.");
        }
    }

    public class LikeCommentModel
    {
        public required long CommentId { get; set; }
    }

    public class LikeCommentModelValidator : AbstractValidator<LikeCommentModel>
    {
        public LikeCommentModelValidator()
        {
            RuleFor(r => r.CommentId)
            .GreaterThan(0).WithMessage("Variable CommentId must be greater than 0.");
        }
    }
}