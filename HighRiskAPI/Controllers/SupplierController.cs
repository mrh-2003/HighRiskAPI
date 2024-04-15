using HighRiskAPI.Models;
using HighRiskAPI.Services;
using HighRiskAPI.ExternalApis;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;

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
        public async Task<ActionResult<JsonDocument>> SearchOfac(string name)
        {
            var result = await WebScrapingAPI.SearchOfac(name);
            return Ok(result);
        }

        [HttpGet("searchOffshoreLeaks/{name}/{country}")]
        public async Task<ActionResult<JsonDocument>> SearchOffshoreLeaks(string name, string country)
        {
            var result = await WebScrapingAPI.SearchOffshoreLeaks(name, country);
            return Ok(result);
        }

        [HttpGet("searchTheWorldBank/{name}/{country}")]
        public async Task<ActionResult<JsonDocument>> SearchTheWorldBank(string name, string country)
        {
            var result = await WebScrapingAPI.SearchTheWorldBank(name, country);
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(long id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
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
            return CreatedAtAction(nameof(GetSupplierById), new { Id = supplier.Id }, supplier);
        }

        // Método PUT para actualizar un proveedor
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(long id, Supplier supplier)
        {
            if (id != supplier.Id)
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
