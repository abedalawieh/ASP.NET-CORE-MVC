using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    [Table(name: "Deliveries")]
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string RecipientName {  get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public Address Address { get; set; } = new();

        [StringLength(50)]
        public string TrackingNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string CarrierName { get; set; } = string.Empty;

        public DateTime? DateDelivery { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; private set; }

        [ForeignKey(nameof(OrderId))]
        public int OrderId { get; set; }


        public void CalculateDeliveryForecast()
        {
            ExpectedDeliveryDate = DateTime.Now.AddDays(14);
        }
    }
}