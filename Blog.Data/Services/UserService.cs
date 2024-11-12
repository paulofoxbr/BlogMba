using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace Blog.Data.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task AddUserToAdminRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roleExists = await _userManager.IsInRoleAsync(user, "Admin");
                if (!roleExists)
                {
                    var result = await _userManager.AddToRoleAsync(user, "Admin");
                    if (result.Succeeded)
                    {
                        // O usuário foi adicionado ao papel "Admin" com sucesso
                        // Aqui você pode adicionar qualquer lógica adicional que desejar
                    }
                }
            }
        }

        public async Task AddUserAsync(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await AddUserToAdminRoleAsync(user.Id);
            }
        }
    }

}
