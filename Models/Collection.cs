using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("collection")]
    public class Collection
    {
        [Key, Column("id", TypeName = "uuid"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required, Column("name", TypeName = "varchar(100)"), MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250),Column("description", TypeName = "varchar(250)")]
        public string? Description { get; set; }

        [Required, ForeignKey("User"), Column("user_id", TypeName = "uuid")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; } = null!;

        [Column("created_at", TypeName = "timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public virtual List<CollectionVerse> Verses { get; set; } = [];


    }
}
