using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.ViewModels
{
    public class ProductVM
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "ISBN-13")]
        [MaxLength(20)]
        public string ISBN13 { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [Display(Name = "List Price")]
        [DataType(DataType.Currency)]
        public decimal PriceList { get; set; }

        [Required]
        [Display(Name = "Standart Price")]
        [DataType(DataType.Currency)]
        public decimal PriceStandart { get; set; }

        [Required]
        [Display(Name = "Price for 50 units or more")]
        [DataType(DataType.Currency)]
        public decimal Price50More { get; set; }

        [Required]
        [Display(Name = "Price for 100 units or more")]
        [DataType(DataType.Currency)]
        public decimal Price100More { get; set; } 

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        public List<ProductImage>? Images { get; set; } = new();
    }
}
