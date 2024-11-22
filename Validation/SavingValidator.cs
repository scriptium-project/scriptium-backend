using FluentValidation;

namespace writings_backend_dotnet.Controllers.Validation
{
    public class SavingProcessModel
    {
        public required VerseValidatedModel Verse { get; set; }
        public required List<string> CollectionNames { get; set; }
    }

    public class SavingProcessModelValidator : AbstractValidator<SavingProcessModel>
    {
        public SavingProcessModelValidator()
        {
            RuleFor(r => r.Verse)
                      .SetValidator(new VerseValidator());

            RuleFor(r => r.CollectionNames)
            .Must(r => r.Count >= 1)
            .WithMessage("You must specify at least one name of your collections.");

            RuleForEach(r => r.CollectionNames)
                        .Must(name => name.Length < 100)
                        .WithMessage("Each collection name must consist of less than 100 characters.");

        }

    }
}