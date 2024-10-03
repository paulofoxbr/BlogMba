using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public AppDbContext()
        {
            
        }

        public DbSet<Models.Author> Authors { get; set; }
        public DbSet<Models.Post> Posts { get; set; }
        public DbSet<Models.Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectonString = "Data Source=localhost,1533;Database=blog;Persist Security Info=True;User ID=sa;Password=Gwnet2007;TrustServerCertificate=True; MultipleActiveResultSets=True";
            optionsBuilder.UseSqlServer(connectonString);
        }
    }
}
