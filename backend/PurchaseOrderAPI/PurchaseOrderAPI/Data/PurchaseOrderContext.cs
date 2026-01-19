using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Models;

namespace PurchaseOrderAPI.Data
{
    public class PurchaseOrderContext : DbContext
    {
        public PurchaseOrderContext(DbContextOptions<PurchaseOrderContext> options)
            : base(options)
        {
        }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
