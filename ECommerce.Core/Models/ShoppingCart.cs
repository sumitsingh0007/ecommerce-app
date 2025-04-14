namespace ECommerce.Core.Models;
public class ShoppingCart
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<CartItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
}

public class CartItem
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}