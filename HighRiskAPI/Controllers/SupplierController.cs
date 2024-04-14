using HighRiskAPI.Models;
using HighRiskAPI.Services;
using HighRiskAPI.ExternalApis;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        // Métodos de búsqueda de la API externa

        [HttpGet("searchOfac/{name}")]
        public async Task<ActionResult<string>> SearchOfac(string name)
        {
            var result = await WebScrapingAPI.SearchOfac(name);
            return Ok(result);
        }

        [HttpGet("searchOffshoreLeaks/{name}")]
        public async Task<ActionResult<string>> SearchOffshoreLeaks(string name)
        {
            var result = await WebScrapingAPI.SearchOffshoreLeaks(name);
            return Ok(result);
        }

        [HttpGet("searchTheWorldBank/{name}")]
        public async Task<ActionResult<string>> SearchTheWorldBank(string name)
        {
            var result = await WebScrapingAPI.SearchTheWorldBank(name);
            return Ok(result);
        }

        // Otros métodos del controlador Supplier

        // Método GET para obtener todos los proveedores
        [HttpGet]
        public async Task<ActionResult<List<Supplier>>> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        // Método GET para obtener un proveedor por su ID
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

        // Métodos POST, PUT y DELETE para agregar, actualizar y eliminar proveedores, respectivamente

        // Método POST para agregar un proveedor
        [HttpPost]
        public async Task<ActionResult<Supplier>> AddSupplier(Supplier supplier)
        {
            await _supplierService.AddSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplierById), new { taxId = supplier.TaxId }, supplier);
        }

        // Método PUT para actualizar un proveedor
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

        // Método DELETE para eliminar un proveedor
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
