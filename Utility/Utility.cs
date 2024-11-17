namespace writings_backend_dotnet.Utility
{
    public static class Utility
    {
        public const string DBTypeDateTime = "datetime";

        public const string DBType8bitInteger = "tinyint";

        public const string DBType16bitInteger = "smallint";

        public const string DBType32bitInteger = "int";

        public const string DBType64bitInteger = "bigint";

        public const string DBTypeVARCHARMAX = "nvarchar(max)";

        public const string DBTypeVARCHAR500 = "VARCHAR(500)";

        public const string DBTypeVARCHAR250 = "VARCHAR(250)";

        public const string DBTypeVARCHAR126 = "VARCHAR(126)";

        public const string DBTypeVARCHAR100 = "VARCHAR(100)";

        public const string DBTypeVARCHAR50 = "VARCHAR(50)";

        public const string DBTypeVARCHAR20 = "VARCHAR(20)";

        public const string DBTypeVARCHAR5 = "VARCHAR(5)";

        public const string DBTypeVARCHAR2 = "VARCHAR(2)";

        public const string DBTypeCHAR1 = "CHAR(1)";

        public const string DBTypeUUID = "uniqueidentifier";

        public const string DBDefaultUUIDFunction = "NEWID()";

        public const string DBDefaultDateTimeFunction = "GETUTCDATE()";


        public const short MIN_LENGTH_FOR_ROOT = 1;
        public const short MAX_LENGTH_FOR_ROOT = 5;
        public class ScriptureDataType : Dictionary<short, ScriptureStructure> { }

        public class ScriptureStructure
        {
            public int SectionCount { get; set; }
            public SectionDataType Sections { get; set; } = [];
        }

        public class SectionDataType : Dictionary<short, SectionStructure> { }

        public class SectionStructure
        {
            public int ChapterCount { get; set; }
            public ChapterDataType Chapters { get; set; } = new ChapterDataType();
        }

        public class ChapterDataType : Dictionary<short, ChapterStructure> { }

        public class ChapterStructure
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
                1, new ScriptureStructure
                {
                    SectionCount = 39,
                    Sections = new SectionDataType
                    {
                        {
                            1, new SectionStructure
                            {
                                ChapterCount = 50,
                                Chapters = new ChapterDataType
                                {
                                    { 1, new ChapterStructure { VerseCount = 19 } }
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}