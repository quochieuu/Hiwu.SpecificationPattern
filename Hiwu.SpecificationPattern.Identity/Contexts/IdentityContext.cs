using Hiwu.SpecificationPattern.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hiwu.SpecificationPattern.Identity.Contexts
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
