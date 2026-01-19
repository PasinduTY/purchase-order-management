using System.ComponentModel.DataAnnotations;
using PurchaseOrderAPI.Enums;

namespace PurchaseOrderAPI.DTOs
{
    public class UpdatePurchaseOrderDto
    {
        [Required]
        [StringLength(50)]
        public string PoNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; }
    }
}