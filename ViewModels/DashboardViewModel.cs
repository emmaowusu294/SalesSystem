using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    /// <summary>
    /// This is the "wrapper" for your main dashboard (Dashboard/Index).
    /// The DashboardController will create this object and fill it with stats.
    /// </summary>
    public class DashboardViewModel
    {
        // --- 1. The KPI (Key Performance Indicator) Cards ---
        // These are the "big numbers" at the top of the page.

        [Display(Name = "Total Revenue")]
        [DisplayFormat(DataFormatString = "{0:C}")] // Formats it as currency (e.g., GH₵1,234.50)
        public decimal TotalRevenue { get; set; }

        [Display(Name = "Total Sales")]
        public int TotalSalesCount { get; set; }

        [Display(Name = "Total Customers")]
        public int TotalCustomersCount { get; set; }

        [Display(Name = "Customer with most sales")]
        public int mostCustomerSales { get; set; }

        [Display(Name = "Total Products")]
        public int TotalProductsCount { get; set; }

        // --- 2. The "Recent Activity" List ---

        // We can re-use the ViewModel we already built for the Sales/Index page!
        public List<SaleListViewModel> RecentSales { get; set; } = new List<SaleListViewModel>();

        // --- 3. Data for Charts (Future) ---

        // We'll leave this empty for now, but you could add
        // properties here to hold chart data.
        // public List<SomeChartData> SalesOverTime { get; set; }``                     

    }
}