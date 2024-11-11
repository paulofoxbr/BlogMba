using Microsoft.AspNetCore.Identity;

namespace Blog.Data.InitializeData
{
    public class InitializeUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        public async Task Initialize(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Certifique-se de que o papel "Admin" exista
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Verifique se o usuário administrador já existe
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                // Crie um novo usuário administrador
                adminUser = new IdentityUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "Admin@123");

                // Adicione o usuário ao papel "Admin"
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

}
