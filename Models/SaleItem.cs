using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSystem.Models
{
    public class SaleItem
    {
        [Key]
        public int SaleItemId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } //price at the time of sale

      

        //foreign keys to sale and products using their ID's
        [Required]
        public int SaleId { get; set; }

        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}