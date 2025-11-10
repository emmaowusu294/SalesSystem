using SalesSystem.Models; // For PaymentMethod
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    /// <summary>
    /// A small "line item" for the receipt.
    /// </summary>
    public class SaleItemDetailViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; } // The frozen price

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal LineTotal { get; set; } // Quantity * UnitPrice
    }

    /// <summary>
    /// The "wrapper" for the entire "Receipt" (Details) page.
    /// </summary>
    public class SaleDetailViewModel
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }

        // Customer Info
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }

        // The list of "line items"
        public List<SaleItemDetailViewModel> Items { get; set; } = new List<SaleItemDetailViewModel>();
    }
}