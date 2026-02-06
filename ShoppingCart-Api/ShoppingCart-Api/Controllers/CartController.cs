using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart_Api.DataLayer;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ShoppingCart_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ProductService _productService;
        public CartController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public IActionResult GetCartProduct()
        {
            // Sample data representing cart items
            var cartItems = new[]
            {
                new { Id = 1, Name = "Item 1", Quantity = 2, Price = 10.0 },
                new { Id = 2, Name = "Item 2", Quantity = 1, Price = 20.0 }
            };
            return Ok(cartItems);
        }
        [HttpGet]
        [Route("products")]
        public IActionResult GetProducts()
        {

        //    var products = new[] {
        //     new { Id = 1, Name = "Product 1", Price = 15.0 },
        //     new { Id = 2,Name = "Product 2", Price = 25.0 },
        //     new { Id = 3,Name = "Product 3", Price = 35.0 }

        //};
        var products = LoadProductsFromFile();
            return Ok(products);
        }

        [HttpPost]
        public IActionResult PostCartItems(CartModel[] carts)
        {
            //var cartItems = new[] {
            //    new { Id = 1,
            //    Name = "Item 1", Quantity = 2, Price = 10.0 },
            //    new { Id = 2, Name = "Item 2", Quantity = 1, Price = 20.0 }
            //};
            return Ok(carts);
        }

        private List<ProductModel> LoadProductsFromFile()
        {
            // var options = new JsonSerializerOptions { PropertyNamingPolicy = new ShortNamePolicy() };
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            //};
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "product.json");
            //var jsonData = System.IO.File.ReadAllText(filePath);
            //return JsonSerializer.Deserialize<List<ProductModel>>(jsonData);
           // return new List<Product>();
           return _productService.GetProducts();
        }
    }

    public class ShortNamePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name.Substring(0, 1).ToLower();
        }
    }

    public class CartModel
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Double Price { get; set; }
    }

    public class ProductModel
    {
       // [JsonPropertyName("i")]
        public int Id { get; set; }
        //[JsonPropertyName("p")]
        public string? ProductName { get; set; }

       // [JsonPropertyName("p")]
        public Double Price { get; set; }
        public string? Description { get; set; }
        //[StringLength(100)]
        public string? Category { get; set; }
    }
}
