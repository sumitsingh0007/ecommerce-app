using ECommerce.Core.Interfaces;
using ECommerce.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class StaticOrderRepository : IRepository<Order>
    {
        private static readonly List<Order> _orders = new();
        private static int _nextId = 1;

        public Task<IEnumerable<Order>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Order>>(_orders);
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return Task.FromResult(_orders.FirstOrDefault(o => o.Id == id));
        }

        public Task AddAsync(Order entity)
        {
            entity.Id = _nextId++;
            _orders.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order entity)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == entity.Id);
            if (existingOrder != null)
            {
                _orders.Remove(existingOrder);
                _orders.Add(entity);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var orderToRemove = _orders.FirstOrDefault(o => o.Id == id);
            if (orderToRemove != null)
            {
                _orders.Remove(orderToRemove);
            }
            return Task.CompletedTask;
        }
    }
}