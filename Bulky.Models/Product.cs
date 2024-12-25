using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string ISBN13 { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal PriceList { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal PriceStandart { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price50More { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price100More { get; set; }

        public int CategoryId { get; set; }

        public List<ProductImage> Images { get; set; } = new();


        [ForeignKey(nameof(CategoryId))]
        public virtual Category? Category { get; set; }
    }
}
