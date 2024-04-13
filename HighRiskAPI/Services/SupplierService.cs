using HighRiskAPI.Models;
using HighRiskAPI.Repositories;

namespace HighRiskAPI.Services
{
    public class SupplierService
    {
        private readonly SupplierRepository _supplierRepository;

        public SupplierService(SupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.GetAllSuppliersAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(long taxId)
        {
            return await _supplierRepository.GetSupplierByIdAsync(taxId);
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            supplier.LastEdition = DateTime.UtcNow; 
            await _supplierRepository.AddSupplierAsync(supplier);
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            var existingSupplier = await _supplierRepository.GetSupplierByIdAsync(supplier.TaxId);
            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found.");
            }

            existingSupplier.BusinessName = supplier.BusinessName;
            existingSupplier.CommercialName = supplier.CommercialName;
            existingSupplier.PhoneNumber = supplier.PhoneNumber;
            existingSupplier.Email = supplier.Email;
            existingSupplier.Website = supplier.Website;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Country = supplier.Country;
            existingSupplier.AnnualBilling = supplier.AnnualBilling;
            existingSupplier.LastEdition = DateTime.UtcNow;

            await _supplierRepository.UpdateSupplierAsync(existingSupplier);
        }

        public async Task DeleteSupplierAsync(long taxId)
        {
            var existingSupplier = await _supplierRepository.GetSupplierByIdAsync(taxId);
            if (existingSupplier == null)
            {
                throw new Exception("Supplier not found.");
            }

            await _supplierRepository.DeleteSupplierAsync(taxId);
        }
    }
}
