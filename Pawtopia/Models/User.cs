using Microsoft.AspNetCore.Identity;

namespace Pawtopia.Models
{
    public class User : IdentityUser
    {
        public string? DisplayName { get; set; }
        public string? ProfileImageLink { get; set; }
        public string Role { get; set; } = "User";
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
