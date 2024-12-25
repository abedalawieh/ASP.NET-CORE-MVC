using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string CNPJ { get; set; } = string.Empty;

        [Required]
        public int AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public virtual Address? Address { get; set; }
    }
}
