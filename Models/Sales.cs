using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSystem.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        public DateTime SaleDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

   
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        // foreign Key for Customer
        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

      
        // many SaleItems can be for a single sale
        public virtual ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}