using Blog.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Configurations;

public static class DatabaseSelectorExtension
{
    public static void AddDatabaseSelector(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite2")));

        }
        else
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }

    }
}


