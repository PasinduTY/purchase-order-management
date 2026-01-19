using System.ComponentModel.DataAnnotations;
using PurchaseOrderAPI.Enums;

namespace PurchaseOrderAPI.DTOs
{
    public class UpdatePurchaseOrderDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SupplierName { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; }
    }
}