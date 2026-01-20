using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PurchaseOrderAPI.DTOs
{
    public class UpdatePurchaseOrderDto
    {
        [Required]
        public string PoNumber { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;
    }
}