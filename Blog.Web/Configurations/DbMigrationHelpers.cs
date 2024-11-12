using Blog.Data.Data;
using Blog.Data.InitializeData;
using Blog.Data.Models;
using Blog.Data.Services;
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
            var newAuthor = await EnsureSeedAuthors(context,userManager);
            await EnsureSeePost(context,newAuthor);
        }
    }

    private static async Task EnsureSeePost(AppDbContext context,Author author)
    {
        var postService = new PostService(context); 

        var countPosts = await postService.GetPostAuthorAsync(1,1);
        if (countPosts.TotalPosts > 0)
        {
            return;
        }

        for (int i = 1; i <= 100; i++)
        {
            var post = new Post
            {
                Id = i,
                Title = $"Poste de número {i}",
                Content = $"Conteúdo do meu post de número {i}",
                AuthorId = author.Id
            };
            postService.CreatePostAsync(post).Wait();
        }
    }

    private static async Task<Author> EnsureSeedAuthors(AppDbContext context,UserManager<IdentityUser> userManager)
    {
        var authorService = new AuthorService(context);
        var newAuthor = new Author();

        newAuthor = await authorService.GetAuthorByUserEmail("autor1@exemplo.com");
        if (newAuthor != null)
        {
            return newAuthor;
        }

        var user = await userManager.FindByEmailAsync("autor1@exemplo.com");
        var userId = user.Id;
        var authors = new Author
        {
            Id = 1,
            Name = "Author 1",
            Email = "autor1@exemplo.com",
            Bio = "Bio 1",
            UserId = userId
        };
        newAuthor = await authorService.CreateAuthorAsync(authors);
        return newAuthor;
    }

    private static async Task EnsureSeedRoles(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("Author"))
        {
            await roleManager.CreateAsync(new IdentityRole("Author"));
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
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        if (await userManager.FindByEmailAsync("autor1@exemplo.com") == null)
        { 
            // criando um usuário sem ser admin para fazer os posts
            var userPost = new IdentityUser
            {
                UserName = "autor1@exemplo.com",
                Email = "autor1@exemplo.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                AccessFailedCount = 0,
                TwoFactorEnabled = false
            };
            var retUsuario = await userManager.CreateAsync(userPost, "User1@123");
            if (retUsuario.Succeeded)
            {
                await userManager.AddToRoleAsync(userPost, "Author");
            }
        }
    }
}
