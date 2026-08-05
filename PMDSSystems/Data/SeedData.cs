using Microsoft.AspNetCore.Identity;

namespace PMDSSystems.Data
{
    public class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider)
        {
            var roleManager =
                serviceProvider.GetRequiredService<
                    RoleManager<IdentityRole>>();

            // =========================
            // CREATE ROLES ONLY
            // =========================
            string[] roles =
            {
                "Admin",
                "Supervisor",
                "Employee"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }
    }
}