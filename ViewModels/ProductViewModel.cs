using SalesSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal SellingPrice { get; set; }

        public int StockQuantity { get; set; }

        public ProductCategory? Category { get; set; }
    }
}
