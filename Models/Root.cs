using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class Root
    {
        [Key, Column("id", TypeName = DBType64bitInteger)]
        public int Id { get; set; }

        [Required, Column("latin", TypeName = DBTypeVARCHAR5), MaxLength(5)]
        public required string Latin { get; set; }

        [Required, Column("own", TypeName = DBTypeVARCHAR5), MaxLength(5)]
        public required string Own { get; set; }

        [Required, Column("scripture_id", TypeName = DBType8bitInteger)]
        public byte ScriptureId { get; set; }

        public virtual Scripture Scripture { get; set; } = null!;

        public virtual List<Word> Words { get; set; } = [];

        [NotMapped]
        public short WordCount => (short)(Words?.Count ?? -1);

    }
}