using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    [Table(name: "PurchaseItems")]
    public class PurchaseItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(OrderId))]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductId))]
        public int ProductId { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public short Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal UnitPrice { get; private set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal TotalPrice { get; private set; }

        public Product Product { get; set; } = null!;
    }
}