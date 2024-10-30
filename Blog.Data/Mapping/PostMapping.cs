using Blog.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mapping
{
    public class PostMapping : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .HasAnnotation("ErrorMessage", "O campo Título é obrigatório.")
                .HasDefaultValue(string.Empty);

            builder.Property(p => p.Content)
                .HasColumnType("NVARCHAR(MAX)") // Set the column type to NVARCHAR(MAX)
                .HasDefaultValue(string.Empty);

            builder.Property(p => p.AuthorId)
                .IsRequired(true)
                .HasAnnotation("ErrorMessage", "O campo AuthorId é obrigatório.")
                .HasColumnType("INT");


        }
    }
}
