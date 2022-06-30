using Microsoft.AspNetCore.Identity;

namespace MyProject.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
