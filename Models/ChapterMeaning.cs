using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class ChapterMeaning
    {
        [Key, Column("id")]
        public int Id { get; set; }

        [Column("chapter_meaning", TypeName = "varchar(50)")]
        public required string Meaning { get; set; }

        [Required]
        public required int ChapterId { get; set; }

        public required Chapter Chapter { get; set; }

        [Required]
        public required short LanguageId { get; set; }

        public required Language Language { get; set; }
    }
}