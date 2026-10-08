using Microsoft.AspNetCore.Identity;
using EventManager.Web.Models;

namespace EventManager.Web.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Organizador", "Participante" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            
            // Dá a permissão de Organizador para a Ana Julia
            var user = await userManager.FindByEmailAsync("anajulia_aninha2@hotmail.com");
            if (user != null && !await userManager.IsInRoleAsync(user, "Organizador"))
            {
                await userManager.AddToRoleAsync(user, "Organizador");
            }

        }
    }
}
