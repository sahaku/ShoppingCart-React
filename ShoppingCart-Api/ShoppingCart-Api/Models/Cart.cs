using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart_Api.Models;

[PrimaryKey("UserId", "ProductId")]
[Table("Cart")]
public partial class Cart
{
    [Key]
    [StringLength(100)]
    public string UserId { get; set; } = null!;

    [Key]
    public int ProductId { get; set; }

    [StringLength(200)]
    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public double Price { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Carts")]
    public virtual Product Product { get; set; } = null!;
}
