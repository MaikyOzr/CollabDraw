using Microsoft.AspNetCore.Identity;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class AppUser: IdentityUser
    {
        public string? Name { get; set; }
        public User? User { get; set; }
    }
}
