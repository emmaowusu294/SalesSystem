// In /ViewModels/SaleAddItemViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    public class SaleAddItemViewModel
    {
        // This will be a hidden field on the form
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // This is just for *displaying* the dropdown
        public SelectList? ProductList { get; set; }
    }
}