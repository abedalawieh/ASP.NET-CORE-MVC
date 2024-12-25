using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public string ApplicationUserId { get; set; } = string.Empty;

        public Payment Payment { get; set; } = new();

        [Required]
        public Delivery Delivery { get; set; } = new();

        [Required]
        public List<PurchaseItem> PurchaseItems { get; set; } = new();

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TotalValue { get; set; }

        [MaxLength(100)]
        public string Status { get; set; } = string.Empty;

        public DateTime AddedIn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; } 

        //------------
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
