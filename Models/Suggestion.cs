using System.ComponentModel.DataAnnotations;

namespace writings_backend_dotnet.Models
{


    public class Suggestion
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public long TranslationTextId { get; set; }

        [Required]
        [MaxLength(500)]
        public required string SuggestionText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
        public TranslationText? TranslationText { get; set; }
    }

}