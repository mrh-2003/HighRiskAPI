using HighRiskAPI.Models;
using HighRiskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HighRiskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }
        [HttpGet("{taxId}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(long taxId)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(taxId);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }
        [HttpPost]
        public async Task<ActionResult<Supplier>> AddSupplier(Supplier supplier)
        {
            await _supplierService.AddSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplierById), new { taxId = supplier.TaxId }, supplier);
        }
        [HttpPut("{taxId}")]
        public async Task<IActionResult> UpdateSupplier(long taxId, Supplier supplier)
        {
            if (taxId != supplier.TaxId)
            {
                return BadRequest();
            }

            try
            {
                await _supplierService.UpdateSupplierAsync(supplier);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
        [HttpDelete("{taxId}")]
        public async Task<IActionResult> DeleteSupplier(long taxId)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(taxId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
