using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PenRealm.Models
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int NovelId { get; set; }

        [ForeignKey("NovelId")]
        public Novel Novel { get; set; }

        public int Order { get; set; }  // Optional: track chapter order
    }
}
