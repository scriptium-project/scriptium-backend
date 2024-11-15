public static class Utility
{
    public const short MIN_LENGTH_FOR_ROOT = 1;
    public const short MAX_LENGTH_FOR_ROOT = 5;


    public class ScriptureDataType : Dictionary<short, Scripture> { }

    public class Scripture
    {
        public int SectionCount { get; set; }
        public SectionDataType Sections { get; set; } = [];
    }

    public class SectionDataType : Dictionary<short, Section> { }

    public class Section
    {
        public int ChapterCount { get; set; }
        public ChapterDataType Chapters { get; set; } = new ChapterDataType();
    }

    public class ChapterDataType : Dictionary<short, Chapter> { }

    public class Chapter
    {
        public int VerseCount { get; set; }
    }

    public static readonly Dictionary<string, int> AvailableScriptures = new()
        {
            { "t", 1 }, // Torah
            { "b", 2 }  // Bible
        };

    public static readonly ScriptureDataType SCRIPTURE_DATA = new()
    {
        {
            1, new Scripture
            {
                SectionCount = 39,
                Sections = new SectionDataType
                {
                    {
                        1, new Section
                        {
                            ChapterCount = 50,
                            Chapters = new ChapterDataType
                            {
                                { 1, new Chapter { VerseCount = 19 } }
                            }
                        }
                    }
                }
            }
        }
    };
}
