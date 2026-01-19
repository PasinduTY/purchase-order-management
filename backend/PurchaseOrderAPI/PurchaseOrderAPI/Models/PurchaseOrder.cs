namespace PurchaseOrderAPI.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PoNumber { get; set; }  // Unique
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Draft/Approved/Shipped/Completed/Cancelled
    }
}
