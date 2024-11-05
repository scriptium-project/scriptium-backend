using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public required User User { get; set; }
        public required TranslationText TranslationText { get; set; }
    }

}