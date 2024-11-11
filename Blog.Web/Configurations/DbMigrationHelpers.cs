using Blog.Data.Data;
using Blog.Data.InitializeData;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Configurations;


public class DbAplyMigrations
{
    private readonly AppDbContext _dbContext;
    public DbAplyMigrations(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        ApplyMigrate().Wait();
    }

    public async Task ApplyMigrate()
    {
        await _dbContext.Database.MigrateAsync();
    }

}

public static class DbMigrationHelperExtension
{
    public static void UseDbMigrationHelper(this WebApplication app) 
    {
        DbMigrationHelpers.EnsureSeedData(app).Wait(); 
    }
}
public static class DbMigrationHelpers
{

    public static async Task EnsureSeedData(WebApplication serviceScope) 
    {
        var services = serviceScope.Services.CreateScope().ServiceProvider;
        await EnsureSeedData(services);
    }

    public static async Task EnsureSeedData(IServiceProvider serviceProvider) 
    {
        using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (env.IsDevelopment() || env.IsEnvironment("Docker") || env.IsStaging())
        {
            await context.Database.MigrateAsync();


            await EnsureSeedRoles(context, roleManager);
            await EnsureSeedUsers(context, userManager);
            //await EnsureSeedAuthors(context);
            //await EnsureSeePost(context);
        }
    }

    private static async Task EnsureSeePost(AppDbContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task EnsureSeedAuthors(AppDbContext context,UserManager<IdentityUser> userManager)
    {
        var user = userManager.FindByEmailAsync("autor1@exemplo.com");
        var userId = user.Id;
        var authors = new Author
        {
            Name = "Author 1",
            Email = "autor1@exemplo.com",
            Bio = "Bio 1",
            UserId = user.Id
        };
    }

    private static async Task EnsureSeedRoles(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }
    private static async Task EnsureSeedUsers(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            // Crie um novo usuário administrador
            adminUser = new IdentityUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TwoFactorEnabled = false
            };
            await userManager.CreateAsync(adminUser, "Admin@123");

            // Adicione o usuário ao papel "Admin"
            await userManager.AddToRoleAsync(adminUser, "Admin");

            // criando um usuário sem ser admin para fazer os posts
            var user = new IdentityUser
            {
                UserName = "usuario@usuario.com",
                Email = "usuario@usuario.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TwoFactorEnabled = false
            };
        }
    }
}
