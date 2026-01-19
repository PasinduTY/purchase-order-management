namespace PurchaseOrderAPI.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PoNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}