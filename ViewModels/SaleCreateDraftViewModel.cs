// In /ViewModels/SaleCreateDraftViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    public class SaleCreateDraftViewModel
    {
        [Required(ErrorMessage = "Please select a customer.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        // This is just for *displaying* the dropdown
        public SelectList? CustomerList { get; set; }
    }
}