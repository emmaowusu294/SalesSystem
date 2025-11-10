using SalesSystem.Models; // For PaymentMethod
using System;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    /// <summary>
    /// A simple ViewModel for showing a single row
    /// in the main "Sales History" (Index) list.
    /// </summary>
    public class SaleListViewModel
    {
        [Display(Name = "Sale ID")]
        public int SaleId { get; set; }

        [Display(Name = "Date")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "Payment")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal TotalAmount { get; set; }
    }
}