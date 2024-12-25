using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class ShopCartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public short Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal UnitPrice { get; private set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TotalPrice { get; private set; }
       
        [Column(TypeName = "datetime2")]
        public DateTime AddedIn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedOn { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required]
        public int ShopCartId { get; set; }

        [ForeignKey(nameof(ShopCartId))]
        public ShopCart? ShopCart { get; set; }


        public void SetUnitPrice(Product product)
        {
            if (product is null) throw new Exception("Invalid Product"); 

            var unitPrice = Quantity switch
            {
                (>= 1 and <= 49) => product.PriceStandart,
                (>= 50 and <= 99) => product.Price50More,
                (>= 100) => product.Price100More,
                _ => throw new Exception("Invalid quantity"),
            };

            UnitPrice = unitPrice;
        }

        public void CalculateTotalPrice()
        {
            TotalPrice = UnitPrice * Quantity;
        }
    }
}
