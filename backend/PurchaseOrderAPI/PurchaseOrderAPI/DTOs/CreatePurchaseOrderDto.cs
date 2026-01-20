using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderAPI.DTOs
{
    public class CreatePurchaseOrderDto
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
    }
}