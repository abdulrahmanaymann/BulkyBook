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
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Science", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Technology", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Business", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Self-Help", DisplayOrder = 5 }
            );

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Tech Solution",
                    StreetAddress = "123 Tech St",
                    City = "Tech City",
                    PostalCode = "12121",
                    State = "IL",
                    PhoneNumber = "6669990000"
                },
                new Company
                {
                    Id = 2,
                    Name = "Vivid Books",
                    StreetAddress = "999 Vid St",
                    City = "Vid City",
                    PostalCode = "66666",
                    State = "IL",
                    PhoneNumber = "7779990000"
                },
                new Company
                {
                    Id = 3,
                    Name = "Readers Club",
                    StreetAddress = "999 Main St",
                    City = "Lala land",
                    PostalCode = "99999",
                    State = "NY",
                    PhoneNumber = "1113335555"
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Description = "A novel set in the Jazz Age that tells the story of the mysterious millionaire Jay Gatsby and his obsession with Daisy Buchanan.",
                    ISBN = "9780743273565",
                    ListPrice = 15.99,
                    Price = 12.99,
                    Price50 = 11.99,
                    Price100 = 10.99,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Title = "Sapiens: A Brief History of Humankind",
                    Author = "Yuval Noah Harari",
                    Description = "An exploration of the history of humanity, from the Stone Age to the present, examining how Homo sapiens came to dominate the world.",
                    ISBN = "9780062316110",
                    ListPrice = 22.99,
                    Price = 19.99,
                    Price50 = 18.49,
                    Price100 = 16.99,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Author = "Robert C. Martin",
                    Description = "A guide for software developers on writing clean, maintainable, and efficient code following agile principles.",
                    ISBN = "9780132350884",
                    ListPrice = 39.99,
                    Price = 34.99,
                    Price50 = 32.99,
                    Price100 = 29.99,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 4,
                    Title = "The Lean Startup: How Today's Entrepreneurs Use Continuous Innovation to Create Radically Successful Businesses",
                    Author = "Eric Ries",
                    Description = "A guide for entrepreneurs on how to build successful startups by applying lean principles and iterative product development.",
                    ISBN = "9780307887894",
                    ListPrice = 24.99,
                    Price = 21.99,
                    Price50 = 20.49,
                    Price100 = 18.99,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 5,
                    Title = "Atomic Habits: An Easy & Proven Way to Build Good Habits & Break Bad Ones",
                    Author = "James Clear",
                    Description = "A practical guide to creating good habits, breaking bad ones, and getting 1% better every day.",
                    ISBN = "9780735211292",
                    ListPrice = 17.99,
                    Price = 15.99,
                    Price50 = 14.49,
                    Price100 = 12.99,
                    CategoryId = 5
                }
            );
        }
    }
}