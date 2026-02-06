using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart_Api.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    public string? ProductName { get; set; }

    public double? Price { get; set; }
    [StringLength(300)]
    public string? Description { get; set; }
    [StringLength(100)]
    public string? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
