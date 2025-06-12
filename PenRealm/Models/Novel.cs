using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PenRealm.Models
{
    public class Novel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Chapter> Chapters { get; set; }
    }
}
