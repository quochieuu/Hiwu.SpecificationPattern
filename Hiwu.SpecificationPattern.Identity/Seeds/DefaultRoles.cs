using Hiwu.SpecificationPattern.Application.Enums;
using Hiwu.SpecificationPattern.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hiwu.SpecificationPattern.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Name = Roles.SuperAdmin.ToString(),
                CreatedDate = DateTime.Now
            });
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Name = Roles.Admin.ToString(),
                CreatedDate = DateTime.Now
            });
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Name = Roles.Moderator.ToString(),
                CreatedDate = DateTime.Now
            });
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Name = Roles.Basic.ToString(),
                CreatedDate = DateTime.Now
            });
        }
    }
}
