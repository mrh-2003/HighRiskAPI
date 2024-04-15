using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HighRiskAPI.Models
{
    public class Supplier
    {
        [Key]
        public required long Id { get; set; }
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener {1} caracteres.")]
        public required string BusinessName { get; set; }
        public required string CommercialName { get; set; }
        public required string TaxId { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Website { get; set; }
        public required string Address { get; set; }
        public required string Country { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public required decimal AnnualBilling { get; set; }
        public required DateTime LastEdition { get; set; }
    }
}
