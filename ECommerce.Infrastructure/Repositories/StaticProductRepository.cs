using ECommerce.Core.Interfaces;
using ECommerce.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class StaticProductRepository : IProductRepository
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Smartphone X", Description = "Latest smartphone with advanced camera", 
                Price = 799.99m, StockQuantity = 50, Category = "Electronics", ImageUrl = "https://media.istockphoto.com/id/2154614902/pl/zdj%C4%99cie/testowanie-smartfon%C3%B3w.jpg?s=1024x1024&w=is&k=20&c=pz0YZj6n9YMtu-fMaPANgHRPZN8xaNMaXLPedfM5P-0=" },
            new Product { Id = 2, Name = "Wireless Headphones", Description = "Noise-cancelling Bluetooth headphones", 
                Price = 199.99m, StockQuantity = 30, Category = "Electronics", ImageUrl = "https://cdn.pixabay.com/photo/2018/09/17/14/27/headphones-3683983_960_720.jpg" },
            new Product { Id = 3, Name = "Organic Cotton T-Shirt", Description = "Comfortable 100% organic cotton t-shirt", 
                Price = 29.99m, StockQuantity = 100, Category = "Clothing", ImageUrl = "https://cdn.pixabay.com/photo/2016/11/23/06/57/isolated-t-shirt-1852114_960_720.png" },
            new Product { Id = 4, Name = "Stainless Steel Water Bottle", Description = "1L insulated water bottle", 
                Price = 24.95m, StockQuantity = 80, Category = "Accessories", ImageUrl = "https://cdn.pixabay.com/photo/2015/05/19/22/32/bottles-774466_960_720.jpg" },
            new Product { Id = 5, Name = "Programming Book", Description = "Complete guide to web development", 
                Price = 39.99m, StockQuantity = 25, Category = "Books", ImageUrl = "https://cdn.pixabay.com/photo/2022/01/22/16/54/book-6957870_960_720.jpg" }
        };

        public Task<IEnumerable<Product>> GetAllAsync() => Task.FromResult<IEnumerable<Product>>(_products);

        public Task<Product?> GetByIdAsync(int id) => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm) => 
            Task.FromResult<IEnumerable<Product>>(_products.Where(p => 
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));

        public Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category) => 
            Task.FromResult<IEnumerable<Product>>(_products.Where(p => 
                p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)));

        // These methods won't persist data but will work for the current session
        public Task AddAsync(Product entity)
        {
            entity.Id = _products.Max(p => p.Id) + 1;
            _products.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Product entity)
        {
            var index = _products.FindIndex(p => p.Id == entity.Id);
            if (index >= 0)
            {
                _products[index] = entity;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }
    }
}