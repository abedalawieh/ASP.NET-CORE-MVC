using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class ShopCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public short QuantityItems => (short)ShopCartItems.Count;

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TotalValue { get; private set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime AddedIn { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime UpdatedOn { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;


        public List<ShopCartItem> ShopCartItems { get; set; } = new List<ShopCartItem>();

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }


        public void CalculateTotalValue()
        {
            TotalValue = ShopCartItems.Sum(s=>s.TotalPrice);
        }
    }
}
