using BulkyBook.Models;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Web Development", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Programming Languages", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Databases", DisplayOrder = 3 },
                new Category { Id = 4, Name = "DevOps", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Software Testing", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Mobile Development", DisplayOrder = 6 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Book 1", Description = "Description 1", ISBN = "ISBN 1", Author = "Author 1", ListPrice = 10.99, Price = 9.99, Price50 = 8.99, Price100 = 7.99, CategoryId = 1, ImageUrl = "" },
                new Product { Id = 2, Title = "Book 2", Description = "Description 2", ISBN = "ISBN 2", Author = "Author 2", ListPrice = 20.99, Price = 19.99, Price50 = 18.99, Price100 = 17.99, CategoryId = 2, ImageUrl = "" },
                new Product { Id = 3, Title = "Book 3", Description = "Description 3", ISBN = "ISBN 3", Author = "Author 3", ListPrice = 30.99, Price = 29.99, Price50 = 28.99, Price100 = 27.99, CategoryId = 3, ImageUrl = "" },
                new Product { Id = 4, Title = "Book 4", Description = "Description 4", ISBN = "ISBN 4", Author = "Author 4", ListPrice = 40.99, Price = 39.99, Price50 = 38.99, Price100 = 37.99, CategoryId = 4, ImageUrl = "" },
                new Product { Id = 5, Title = "Book 5", Description = "Description 5", ISBN = "ISBN 5", Author = "Author 5", ListPrice = 50.99, Price = 49.99, Price50 = 48.99, Price100 = 47.99, CategoryId = 2, ImageUrl = "" },
                new Product { Id = 6, Title = "Book 6", Description = "Description 6", ISBN = "ISBN 6", Author = "Author 6", ListPrice = 60.99, Price = 59.99, Price50 = 58.99, Price100 = 57.99, CategoryId = 1, ImageUrl = "" }
            );
        }
    }
}
