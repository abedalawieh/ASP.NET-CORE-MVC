using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;

        public Payment Payment { get; set; } = new();

        public Delivery Delivery { get; set; } = new();

        public List<PurchaseItem> PurchaseItems { get; set; } = new();

        public decimal TotalValue { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime AddedIn { get; set; }

        public DateTime UpdatedOn { get; set; }

        //------------
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
