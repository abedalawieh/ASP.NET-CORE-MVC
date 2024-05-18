using MVCProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MVCProject.DataAccess.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 1 },
                new Category { Id = 3, Name = "History", DisplayOrder = 1 }

                );
             modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Fortune of Time", Author = "Billy Spark", Description = "Good Book", ISBN = "SWD99991", ListPrice = 99, Price = 90, Price50 = 85, Price100 = 80,CategoryId=6,ImageUrl="" },
                new Product { Id = 2, Title = "Poor Dad Rich Dad", Author = "Abed Alawieh", Description = "Good Book", ISBN = "SWD99891", ListPrice = 95, Price = 89, Price50 = 84, Price100 = 79, CategoryId = 6, ImageUrl = "" },
                new Product { Id = 3, Title = "Game Of Thrones", Author = "Roman Repelski", Description = "Bad Book", ISBN = "SWD95991", ListPrice = 89, Price = 85, Price50 = 80, Price100 = 75, CategoryId = 6 ,ImageUrl = "" }

                );
        }
    }
}
