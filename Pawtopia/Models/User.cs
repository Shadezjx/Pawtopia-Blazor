using Microsoft.AspNetCore.Identity;

namespace Pawtopia.Models
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = String.Empty;
        public string ProfileImageLink { get; set; } = String.Empty;

        public ICollection<IdentityUserRole<string>> UserRoles { get; set; } = [];

        public ICollection<IdentityRole> Roles { get; set; } = [];

        public ICollection<Address> Addresses { get; set; } = [];
    }
}
