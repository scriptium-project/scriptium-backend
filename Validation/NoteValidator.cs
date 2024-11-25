using FluentValidation;
using Ganss.Xss;
using HtmlAgilityPack;

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
    public class NoteModel
    {
        public required string NoteText { get; set; }
        public required VerseValidatedModel Verse { get; set; }
    }

    public class NoteModelValidator : AbstractValidator<NoteModel>
    {
        public NoteModelValidator()
        {
            RuleFor(r => r.NoteText).NotEmpty().WithMessage("NoteText must not be empty.")
                .MinimumLength(1).WithMessage("NoteText must not be empty.")
                .Must(BeValidHtml).WithMessage("NoteText contains invalid content.");

            RuleFor(r => r.Verse)
           .SetValidator(new VerseValidator());
        }

        private bool BeValidHtml(string noteText)
        {
            //Phase 1: HTML Validation
            HtmlSanitizer sanitizer = new();

            //Allowed tags, attributes and css properties.
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("u");
            sanitizer.AllowedTags.Add("span");
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.Add("style");
            sanitizer.AllowedCssProperties.Add("color");

            string sanitized = sanitizer.Sanitize(noteText);

            bool isValid = sanitized == noteText;

            if (!isValid) return false;

            //Phase 2: Plain text constraint.
            HtmlDocument doc = new();

            doc.LoadHtml(noteText);
            string plainText = doc.DocumentNode.InnerText;

            isValid = plainText.Length <= 1000;

            if (!isValid) return false;
            //Phase 3: HasNested tag validation.
            return HasNestedTags(doc.DocumentNode);
        }

        private bool HasNestedTags(HtmlNode node)
        {
            if (node == null)
                return false;

            foreach (var child in node.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Element)
                {
                    if (node.ParentNode != null && node.ParentNode.Name != "#document")
                        return true;

                    if (HasNestedTags(child))
                        return true;
                }
            }
            return false;
        }
    }
}