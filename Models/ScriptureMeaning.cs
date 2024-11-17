

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Models
{
    public class ScriptureMeaning
    {
        [Key, Column("id", TypeName = DBType16bitInteger)]
        public required int Id { get; set; }

        [Required, Column("meaning",TypeName = DBTypeVARCHAR50)]
        public required string Meaning { get; set; }

        [Required, Column("scripture_id", TypeName = DBType8bitInteger)]
        public byte ScriptureId { get; set; }
        public virtual Scripture Scripture { get; set; }  = null!;

        [Required, Column("language_id", TypeName = DBType8bitInteger)]
        public byte LanguageId { get; set; }
        public virtual Language Language { get; set; }  = null!;

    }
}