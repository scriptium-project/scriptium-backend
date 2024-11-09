using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{
    public class LikeNote
    {
        [Key]
        public long LikeId { get; set; }

        [Required]
        public int NoteId { get; set; }

        public Like? Like { get; set; }

        public Note? Note { get; set; }
    }
}