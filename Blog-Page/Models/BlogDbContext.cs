using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlogNET.Models
{
    public partial class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {
        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Newsletter> Newsletter { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=EMIR\\SQLEXPRESS01;Database=BlogDb;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasIndex(e => e.AuthorId);

                entity.HasIndex(e => e.CategoryId);

                entity.Property(e => e.CreateTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Blog)
                    .HasForeignKey(d => d.AuthorId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                //entity.HasNoKey();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Newsletter>(entity =>
            {
                //entity.HasNoKey();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
