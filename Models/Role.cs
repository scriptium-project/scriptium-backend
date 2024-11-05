using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace writings_backend_dotnet.Models
{
    public class Role
    {
        [Key, Column("id")]
        public short Id { get; set; }

        [Required, Column("role_name")]
        public required string RoleName { get; set; }

        public string? Description { get; set; } = null!;

        public virtual List<User> Users { get; set; } = [];
    }
}