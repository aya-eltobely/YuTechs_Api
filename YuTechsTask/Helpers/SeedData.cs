using Microsoft.AspNetCore.Identity;
using YuTechsTask.Models;

namespace YuTechsTask.Helpers
{
    public static class SeedData
    {

        public static void Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDBContext>();


            //create Roles
            if (!roleManager.RoleExistsAsync(WebSiteRoles.SiteAdmin).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteAdmin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteAuthor)).GetAwaiter().GetResult();
            }

            //Create Admin
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
            }, "Admin123#").GetAwaiter().GetResult();

            //check admin exist
            var AppAdmin = userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult();

            //asign role to admin
            if (AppAdmin != null)
            {
                userManager.AddToRoleAsync(AppAdmin, WebSiteRoles.SiteAdmin).GetAwaiter().GetResult();
            }

            //////////////    

            dbContext.SaveChanges();




        }

    }
}
