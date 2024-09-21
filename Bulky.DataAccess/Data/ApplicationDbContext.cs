using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Web Development", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Programming Languages", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Databases", DisplayOrder = 3 },
                new Category { Id = 4, Name = "DevOps", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Software Testing", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Mobile Development", DisplayOrder = 6 }
            );
        }
    }
}
