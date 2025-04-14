using ECommerce.Core.Models;
namespace ECommerce.Core.Interfaces;
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
}