using Microsoft.AspNetCore.Identity;

namespace PenRealm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Novel> Novels { get; set; }
    }
}
