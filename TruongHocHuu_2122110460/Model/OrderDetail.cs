using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruongHocHuu_2122110460.Model
{
    public class OrderDetail
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public DateTime createdAt { get; set; }
    }
}
