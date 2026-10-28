using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PMDSSystems.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // =========================
            // CREATE ROLES
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
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // =========================
            // CREATE ADMIN USER
            // =========================
            string adminEmail = "admin@pmds.com";
            string adminPassword = "Admin@12345"; // You can change this

            // Check if admin exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Create admin user
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"Admin user created: {adminEmail}");
                    Console.WriteLine($"Password: {adminPassword}");
                }
                else
                {
                    Console.WriteLine("Failed to create admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Admin user already exists: {adminEmail}");
            }

            // =========================
            // CREATE A DEFAULT EMPLOYEE (Optional)
            // =========================
            string employeeEmail = "employee@pmds.com";
            string employeePassword = "Employee@12345";

            var employeeUser = await userManager.FindByEmailAsync(employeeEmail);

            if (employeeUser == null)
            {
                employeeUser = new IdentityUser
                {
                    UserName = employeeEmail,
                    Email = employeeEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(employeeUser, employeePassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employeeUser, "Employee");
                    Console.WriteLine($"Employee user created: {employeeEmail}");
                    Console.WriteLine($"Password: {employeePassword}");
                }
            }

            // =========================
            // CREATE A SUPERVISOR (Optional)
            // =========================
            string supervisorEmail = "supervisor@pmds.com";
            string supervisorPassword = "Supervisor@12345";

            var supervisorUser = await userManager.FindByEmailAsync(supervisorEmail);

            if (supervisorUser == null)
            {
                supervisorUser = new IdentityUser
                {
                    UserName = supervisorEmail,
                    Email = supervisorEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    LockoutEnabled = false
                };

                var result = await userManager.CreateAsync(supervisorUser, supervisorPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(supervisorUser, "Supervisor");
                    Console.WriteLine($"Supervisor user created: {supervisorEmail}");
                    Console.WriteLine($"Password: {supervisorPassword}");
                }
            }

            Console.WriteLine("Seed data completed!");
        }
    }
}