using System;
using Microsoft.EntityFrameworkCore;

namespace AdminBlog.Models{

    public class BlogContext : DbContext
    { // DbContext EntityFrameworkCore referans edilerek kullanılıyor.
        public BlogContext(DbContextOptions<BlogContext> options) : base(options){ // burası constractor

        }
        // bu alttakiler tablosunu oluşturmak istediğimiz sınıflar
        public DbSet<Author> Author{get;set;}
        public DbSet<Category> Category{get;set;}
        public DbSet<Blog> Blog{get;set;}
    }
}