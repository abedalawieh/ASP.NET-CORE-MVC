using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.ViewModels
{
    public class ShopCartVM
    {
        [Key]
        public int Id { get; set; }

        public List<ShopCartItem> ShopCartItems { get; set; } = new List<ShopCartItem>();

        [Required]
        public short QuantityItems => (short)ShopCartItems.Count;

        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser? ApplicationUser { get; set; }

        public Delivery Delivery { get; set; } = new();
    }
}
