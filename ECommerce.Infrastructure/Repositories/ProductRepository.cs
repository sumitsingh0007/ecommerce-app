using ECommerce.Core.Interfaces;
using ECommerce.Core.Models;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();
    public async Task<Product?> GetByIdAsync(int id) => await _context.Products.FindAsync(id);
    public async Task AddAsync(Product entity) => await _context.Products.AddAsync(entity);
    public async Task UpdateAsync(Product entity) => _context.Products.Update(entity);
    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null) _context.Products.Remove(product);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm) =>
        await _context.Products
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category) =>
        await _context.Products.Where(p => p.Category == category).ToListAsync();
}
