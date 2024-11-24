using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace writings_backend_dotnet.Models
{
    [Table("collection_verse")]
    public class CollectionVerse
    {
        [Key, Column("id", TypeName = Utility.DBType64bitInteger)]
        public long Id { get; set; }

        [Required, ForeignKey("Collection"), Column("collection_id", TypeName = Utility.DBTypeUUID)]
        public Guid CollectionId { get; set; }

        public virtual Collection Collection { get; set; } = null!;

        [Required, ForeignKey("Verse"), Column("verse_id")]
        public int VerseId { get; set; }

        public virtual Verse Verse { get; set; } = null!;

        [Column("saved_at", TypeName = Utility.DBTypeDateTime)]
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(250), Column("note", TypeName = Utility.DBTypeVARCHAR250)]
        public string? Note { get; set; }
    }
}
