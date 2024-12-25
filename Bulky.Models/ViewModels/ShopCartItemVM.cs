using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.ViewModels
{
    public class ShopCartItemVM
    {
        public int? Id { get; set; }

        [Required]
        [Range(1, short.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public short Quantity { get; set; } = 1;

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product? Product { get; set; }

        [Required]
        public int ShopCartId { get; set; }

        public ShopCart? ShopCart { get; set; } 
    }
}
