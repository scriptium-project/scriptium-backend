using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    public class FootNoteText
    {
        [Key, Column("id", TypeName = Utility.DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("text", TypeName = Utility.DBTypeVARCHARMAX)]
        public string Text { get; set; } = null!;

        public virtual List<FootNote>? FootNotes { get; set; }
    }
}
