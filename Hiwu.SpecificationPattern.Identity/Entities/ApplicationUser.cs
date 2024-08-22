using Microsoft.AspNetCore.Identity;

namespace Hiwu.SpecificationPattern.Identity.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public short? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
