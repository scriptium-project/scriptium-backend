using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace scriptium_backend_dotnet.Models.Util
{
    public class UserUpdateR
    {
        [Key, Column("id", TypeName = Utility.DBType64bitInteger)]
        public long Id { get; set; }

        [Required, ForeignKey("UserId"), Column("user_id")]
        public required Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [MaxLength(16)]
        public string? Username { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        [MaxLength(30)]
        public string? Surname { get; set; }

        public byte[]? Image { get; set; }

        [MaxLength(1)]
        public string? Gender { get; set; }

        [MaxLength(256)]
        public string? Biography { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Column("preferred_languageId", TypeName = Utility.DBType8bitInteger)]
        public byte? PreferredLanguageId { get; set; }

    }
}