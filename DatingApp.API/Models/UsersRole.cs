using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Models
{
    public class UsersRole : IdentityUserRole<int>
    {
        public Users User { get; set; }
        public Role Role { get; set; }
    }
}