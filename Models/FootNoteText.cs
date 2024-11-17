using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class FootNoteText
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public long Id { get; set; }

        [Required, Column("text", TypeName = DBTypeVARCHARMAX)]
        public string Text { get; set; } = null!;

        public virtual List<FootNote>? FootNotes { get; set; }
    }
}
