using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class Root
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Required, MaxLength(5)]
        public required string Latin { get; set; }
        [Required, MaxLength(5)]
        public required string Own { get; set; }
        [Required, Column("scripture_id", TypeName = "smallint")]
        public required short ScriptureId { get; set; }
        public required Scripture Scripture { get; set; }
        [NotMapped]
        public short WordCount => (short)(Words?.Count ?? -1);
        public List<Word> Words { get; set; } = [];


    }
}