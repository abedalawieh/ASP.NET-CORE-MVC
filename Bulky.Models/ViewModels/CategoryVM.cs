using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.ViewModels
{
    public class CategoryVM
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        [DisplayName("Category Name")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 100)]
        [DisplayName("Display Order")]
        public byte DisplayOrder { get; set; }
    }
}
