using Microsoft.AspNetCore.Identity;

namespace Hiwu.SpecificationPattern.Identity.Entities
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
