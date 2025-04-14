
using ECommerce.Core.Models;
namespace ECommerce.Core.Interfaces;
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(string customerId);
    Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate);
}