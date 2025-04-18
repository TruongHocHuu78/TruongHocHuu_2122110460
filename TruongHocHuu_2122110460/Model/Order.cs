using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruongHocHuu_2122110460.Model
{
    public class Order
    {
        public long Id { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public User? User { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

    public enum OrderStatus
    {
        PENDING,
        PREPARING,
        DELIVERED,
        CANCELLED
    }

    public enum PaymentMethod
    {
        CASH,
        ONLINE
    }
    
}
