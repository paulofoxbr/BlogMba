using Blog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mapping
{
    public class AuthorMapping : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .HasAnnotation("ErrorMessage","O campo Nome é obrigatório.")
                .HasDefaultValue(string.Empty);

            builder.Property(a => a.Email)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")                
                .HasMaxLength(254)
                .HasAnnotation("ErrorMessage", "O campo Email é obrigatório.")
                .HasDefaultValue(string.Empty);

            builder.Property(a => a.Bio)
                .IsRequired(false)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(1000)
                .HasDefaultValue(string.Empty);

            builder.Property(a => a.UserId)
                //.IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(450)
                //.HasAnnotation("ErrorMessage", "O campo UserId é obrigatório.")
                .HasDefaultValue(string.Empty);
 
        }
    }
}
