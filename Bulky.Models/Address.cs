using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    [Table(name: "Addresses")]
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PostalCode { get; set; } = string.Empty;
    }
}
