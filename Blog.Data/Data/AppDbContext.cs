using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Data
{
    public class AppDbContext : IdentityDbContext //<ApplicationUser>
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

            modelBuilder.Entity<Models.Author>();
            modelBuilder.Entity<Models.Post>();
            modelBuilder.Entity<Models.Comment>();
            modelBuilder.Entity<Author>().HasMany(a => a.Posts).WithOne(p => p.Author).HasForeignKey(p => p.AuthorId);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectonString = "Data Source=localhost,1533;Database=blog;Persist Security Info=True;User ID=sa;Password=Gwnet2007;TrustServerCertificate=True; MultipleActiveResultSets=True";
        //    optionsBuilder.UseSqlServer(connectonString);
        //}
    }


}
