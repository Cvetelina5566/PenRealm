using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PenRealm.Models
{
    public class Poem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsPublic { get; set; } = true;
        public string AuthorId { get; set; } = null!;

        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; } = null!;
    }
}
