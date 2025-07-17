using GoDecola.API.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Data
{
    public class SeedData
    {
        public static async Task Initialize(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!await roleManager.RoleExistsAsync("ADMIN"))
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }

            if (!await roleManager.RoleExistsAsync("ATTENDANT"))
            {
                await roleManager.CreateAsync(new IdentityRole("ATTENDANT"));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@godecola.com");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin@godecola.com",
                    Email = "admin@godecola.com",
                    FirstName = "Admin",
                    LastName = "GoDecola",
                };

                var resultado = await userManager.CreateAsync(adminUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
            }

            var attendantUser = await userManager.FindByEmailAsync("attendant@godecola.com");

            if (attendantUser == null)
            {
                attendantUser = new User
                {
                    UserName = "attendant@godecola.com",
                    Email = "attendant@godecola.com",
                    FirstName = "Attendant",
                    LastName = "GoDecola",
                };

                var resultado = await userManager.CreateAsync(attendantUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(attendantUser, "ATTENDANT");
                }
            }
        }
    }
}
