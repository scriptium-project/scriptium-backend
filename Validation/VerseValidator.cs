using FluentValidation;


namespace writings_backend_dotnet.Controllers.Validation
{
    public class VerseValidatedModel
    {
        public required short ScriptureNumber { get; set; }
        public required short SectionNumber { get; set; }
        public required short ChapterNumber { get; set; }
        public required short VerseNumber { get; set; }
    }

    public class VerseValidator : AbstractValidator<VerseValidatedModel>
    {
        public VerseValidator()
        {
            RuleFor(x => x.ScriptureNumber)
                    .Cascade(CascadeMode.Stop)
                    .Must(num => num >= 1)
                    .WithMessage("Scripture number is too small; minimum is 1.")
                    .Must(Utility.SCRIPTURE_DATA.ContainsKey)
                    .WithMessage(x => $"Scripture number {x.ScriptureNumber} is not valid.");

            RuleFor(x => x.SectionNumber)
                .Cascade(CascadeMode.Stop)
                .Must(num => num >= 1)
                    .WithMessage("Section number is too small; minimum is 1.")
                .Must((dto, sectionNumber) =>
                {
                    if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture))
                        return sectionNumber <= scripture.SectionCount;
                    return false;
                })
                    .WithMessage(dto => $"Section number is too big; maximum is {GetSectionCount(dto)} in scripture {dto.ScriptureNumber}.");

            RuleFor(x => x.ChapterNumber)
                .Cascade(CascadeMode.Stop)
                .Must(num => num >= 1)
                    .WithMessage("Chapter number is too small; minimum is 1.")
                .Must((dto, chapterNumber) =>
                {
                    if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture) &&
                        scripture.Sections.TryGetValue(dto.SectionNumber, out var section))
                        return chapterNumber <= section.ChapterCount;

                    return false;
                })
                    .WithMessage(dto => $"Chapter number is too big; maximum is {GetChapterCount(dto)} in section {dto.SectionNumber} of scripture {dto.ScriptureNumber}.");

            RuleFor(x => x.VerseNumber)
                .Cascade(CascadeMode.Stop)
                .Must(num => num >= 1)
                    .WithMessage("Verse number is too small; minimum is 1.")
                .Must((dto, verseNumber) =>
                {
                    if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture) &&
                        scripture.Sections.TryGetValue(dto.SectionNumber, out var section) &&
                        section.Chapters.TryGetValue(dto.ChapterNumber, out var chapter))
                        return verseNumber <= chapter.VerseCount;

                    return false;
                })
                    .WithMessage(dto => $"Verse number is too big; maximum is {GetVerseCount(dto)} in chapter {dto.ChapterNumber}.");
        }

        private int GetSectionCount(VerseValidatedModel dto)
        {
            if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture))
                return scripture.SectionCount;

            return -1;
        }

        private int GetChapterCount(VerseValidatedModel dto)
        {
            if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture) &&
                scripture.Sections.TryGetValue(dto.SectionNumber, out var section))
                return section.ChapterCount;

            return -1;
        }

        private int GetVerseCount(VerseValidatedModel dto)
        {
            if (Utility.SCRIPTURE_DATA.TryGetValue(dto.ScriptureNumber, out var scripture) &&
                scripture.Sections.TryGetValue(dto.SectionNumber, out var section) &&
                section.Chapters.TryGetValue(dto.ChapterNumber, out var chapter))
                return chapter.VerseCount;
            return -1;
        }
    }
}
