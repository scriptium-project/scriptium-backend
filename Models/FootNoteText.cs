using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class FootNoteText
    {
        [Key, Column("id", TypeName = "bigint"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required, MaxLength(4000)]
        public string Text { get; set; } = null!;

        public List<FootNote>? FootNotes { get; set; }
    }
}
