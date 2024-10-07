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
    public class CommentMapping : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.PostComment)
                .IsRequired(true)
                .HasColumnType("NVARCHAR")
                .HasMaxLength(500)
                .HasAnnotation("ErrorMessage", "O campo Conteúdo é obrigatório.")
                .HasDefaultValue(string.Empty);

            builder.Property(c => c.PostId)
                .IsRequired(true)
                .HasAnnotation("ErrorMessage", "O campo PostId é obrigatório.")
                .HasColumnType("INT");

            builder.Property(c => c.AuthorId)
                .IsRequired(true)
                .HasAnnotation("ErrorMessage", "O campo AuthorId é obrigatório.")
                .HasColumnType("INT");
        }


    }
}
