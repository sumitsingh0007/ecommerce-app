using ECommerce.Core.Interfaces;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrdersController(IRepository<Order> orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        // Validate products and calculate total
        decimal total = 0;
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null) return BadRequest($"Product {item.ProductId} not found");
            if (product.StockQuantity < item.Quantity) return BadRequest($"Insufficient stock for product {product.Name}");
            
            item.UnitPrice = product.Price;
            total += item.Quantity * product.Price;
        }

        order.TotalAmount = total;
        await _orderRepository.AddAsync(order);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }
}