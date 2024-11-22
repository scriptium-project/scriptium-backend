using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class NoteIdentifierModel
    {
        public required long NoteId { get; set; }
    }

    public class NoteIdentifierModelValidator : AbstractValidator<NoteIdentifierModel>
    {
        public NoteIdentifierModelValidator()
        {
            RuleFor(r => r.NoteId)
            .GreaterThan(0).WithMessage("Variable NoteId must be greater than 0.");
        }
    }


    public class NoteUpdateModel
    {
        public required long NoteId { get; set; }

        public required string NewNoteText { get; set; }
    }

    public class NoteUpdateModelValidator : AbstractValidator<NoteUpdateModel>
    {
        public NoteUpdateModelValidator()
        {
            RuleFor(r => r.NoteId)
            .GreaterThan(0).WithMessage("Variable NoteId must be greater than 0.");

            //TODO: Rule  for NewNoteText property will be implemented.
        }
    }


    public class NoteCreateModel
    {
        public required string NoteText { get; set; }
        public required VerseValidatedModel Verse { get; set; }

    }

    public class NoteCreateModelValidator : AbstractValidator<NoteCreateModel>
    {
        public NoteCreateModelValidator()
        {
            RuleFor(r => r.NoteText)
            .MinimumLength(1).WithMessage("NoteText must not be empty.");

            RuleFor(r => r.Verse)
           .SetValidator(new VerseValidator());
        }
    }
}