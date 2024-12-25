using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bulky.Models
{
    [Table(name: "Payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(OrderId))]
        public int OrderId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public DateTime? PaymentDueDate { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public string PaymentIntentId { get; set; } = string.Empty;

        [MaxLength(100)]
        public string PaymentStatus { get; set; } = string.Empty;
    }
}