using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Data;
using PurchaseOrderAPI.Enums;
using PurchaseOrderAPI.Models;

namespace PurchaseOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly PurchaseOrderContext _context;

        public PurchaseOrdersController(PurchaseOrderContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrders
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders(
            string? supplier,
            PurchaseOrderStatus? status,
            string sortBy = "OrderDate",
            string sortDir = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.PurchaseOrders.AsQueryable();

            // 🔍 Filtering
            if (!string.IsNullOrWhiteSpace(supplier))
                query = query.Where(p => p.SupplierName.Contains(supplier));

            if (status.HasValue)
                query = query.Where(p => p.Status == status);

            // ↕ Sorting
            query = sortBy.ToLower() switch
            {
                "suppliername" => sortDir == "desc"
                    ? query.OrderByDescending(p => p.SupplierName)
                    : query.OrderBy(p => p.SupplierName),

                "totalamount" => sortDir == "desc"
                    ? query.OrderByDescending(p => p.TotalAmount)
                    : query.OrderBy(p => p.TotalAmount),

                _ => sortDir == "desc"
                    ? query.OrderByDescending(p => p.OrderDate)
                    : query.OrderBy(p => p.OrderDate)
            };

            // 📄 Pagination
            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalRecords,
                page,
                pageSize,
                data
            });
        }


        // GET: api/PurchaseOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null)
                return NotFound();

            return po;
        }

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> CreatePurchaseOrder(PurchaseOrder po)
        {
            po.Status = PurchaseOrderStatus.Draft; // default
            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = po.Id }, po);
        }

        // PUT: api/PurchaseOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, PurchaseOrder po)
        {
            if (id != po.Id)
                return BadRequest();

            _context.Entry(po).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PurchaseOrders.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/PurchaseOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null)
                return NotFound();

            _context.PurchaseOrders.Remove(po);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
