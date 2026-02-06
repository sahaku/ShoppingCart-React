using ShoppingCart_Api.Controllers;
using ShoppingCart_Api.Models;

namespace ShoppingCart_Api.DataLayer
{
    public class ProductService
    {
        private readonly ShoppingContext _context;
        public ProductService(ShoppingContext context)
        {
            _context = context;
        }
        public List<ProductModel> GetProducts()
        {
            return _context.Products.Select(p => new ProductModel
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price ?? 0,
                Category = p.Category,
                Description = p.Description
            }).ToList();
        }
    }
}
