using HighRiskAPI.Context;
using HighRiskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HighRiskAPI.Repositories
{
    public class SupplierRepository
    {
        private readonly AppDbContext _context;

        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers.OrderByDescending(s => s.LastEdition).ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(long id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplierAsync(long id)
        {
            var supplierToDelete = await _context.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
            if (supplierToDelete != null)
            {
                _context.Suppliers.Remove(supplierToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
