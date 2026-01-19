using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Data;
using PurchaseOrderAPI.DTOs;
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
                .Select(p => new PurchaseOrderDto
                {
                    Id = p.Id,
                    PoNumber = p.PoNumber,
                    Description = p.Description,
                    SupplierName = p.SupplierName,
                    OrderDate = p.OrderDate,
                    TotalAmount = p.TotalAmount,
                    Status = p.Status.ToString()
                })
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
        public async Task<ActionResult<PurchaseOrderDto>> GetPurchaseOrder(int id)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null)
                return NotFound();

            var dto = new PurchaseOrderDto
            {
                Id = po.Id,
                PoNumber = po.PoNumber,
                Description = po.Description,
                SupplierName = po.SupplierName,
                OrderDate = po.OrderDate,
                TotalAmount = po.TotalAmount,
                Status = po.Status.ToString()
            };

            return dto;
        }

        // POST: api/PurchaseOrders
        [HttpPost]
        public async Task<ActionResult<PurchaseOrderDto>> CreatePurchaseOrder(CreatePurchaseOrderDto dto)
        {
            var po = new PurchaseOrder
            {
                PoNumber = dto.PoNumber,
                Description = dto.Description,
                SupplierName = dto.SupplierName,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                Status = PurchaseOrderStatus.Draft
            };

            _context.PurchaseOrders.Add(po);
            await _context.SaveChangesAsync();

            var resultDto = new PurchaseOrderDto
            {
                Id = po.Id,
                PoNumber = po.PoNumber,
                Description = po.Description,
                SupplierName = po.SupplierName,
                OrderDate = po.OrderDate,
                TotalAmount = po.TotalAmount,
                Status = po.Status.ToString()
            };

            return CreatedAtAction(nameof(GetPurchaseOrder), new { id = po.Id }, resultDto);
        }

        // PUT: api/PurchaseOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePurchaseOrder(int id, UpdatePurchaseOrderDto dto)
        {
            var po = await _context.PurchaseOrders.FindAsync(id);
            if (po == null)
                return NotFound();

            po.PoNumber = dto.PoNumber;
            po.Description = dto.Description;
            po.SupplierName = dto.SupplierName;
            po.OrderDate = dto.OrderDate;
            po.TotalAmount = dto.TotalAmount;
            po.Status = dto.Status;

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
