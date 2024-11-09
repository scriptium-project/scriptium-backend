

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class ScriptureMeaning
    {
        [Key]
        public required int Id { get; set; }

        [Required, Column(TypeName = "varchar(50)")]
        public required string Meaning { get; set; }

        [Required, Column("scripture_id")]
        public short ScriptureId { get; set; }

        public required Scripture Scripture { get; set; }

        [Required, Column("language_id")]
        public short LanguageId { get; set; }

        public required Language Language { get; set; }

    }
}