using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class SectionMeaning
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required, Column("meaning", TypeName = DBTypeVARCHAR100)]
        public required string Meaning { get; set; }

        [Required,Column("section_id", TypeName = DBType16bitInteger)]
        public short SectionId { get; set; }

        public virtual Section Section { get; set; }  = null!;

        [Required, Column("language_id", TypeName = DBType8bitInteger)]
        public byte LanguageId { get; set; }

        public virtual Language Language { get; set; }  = null!;

    }
}