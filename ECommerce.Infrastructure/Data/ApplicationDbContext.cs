using ECommerce.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Smartphone X", Description = "Latest smartphone with advanced camera", 
                Price = 799.99m, StockQuantity = 50, Category = "Electronics", ImageUrl = "https://example.com/images/phone.jpg" },
            new Product { Id = 2, Name = "Wireless Headphones", Description = "Noise-cancelling Bluetooth headphones", 
                Price = 199.99m, StockQuantity = 30, Category = "Electronics", ImageUrl = "https://example.com/images/headphones.jpg" },
            new Product { Id = 3, Name = "Organic Cotton T-Shirt", Description = "Comfortable 100% organic cotton t-shirt", 
                Price = 29.99m, StockQuantity = 100, Category = "Clothing", ImageUrl = "https://example.com/images/tshirt.jpg" }
        );
    }
}