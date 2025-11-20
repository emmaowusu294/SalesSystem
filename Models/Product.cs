using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; } 

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }

        // A Product can be in many SaleItems
        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}