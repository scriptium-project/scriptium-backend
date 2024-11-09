using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class SectionMeaning
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Required]
        public required string Meaning { get; set; }

        [Required]
        public required short SectionId { get; set; }

        public required Section Section { get; set; }

        public short LanguageId { get; set; }

        public required Language Language { get; set; }

    }
}