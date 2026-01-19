using PurchaseOrderAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderAPI.DTOs
{
    public class CreatePurchaseOrderDto
    {
        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    }
}