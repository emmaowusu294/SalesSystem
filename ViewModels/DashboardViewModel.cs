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

        //top customer with more sales
        [Display(Name = "Top Customer - by sales")]
        public string TopCustomerName { get; set; }

        [Display(Name = "Top Customer Sales")]
        public int TopCustomerSalesCount { get; set; }

        //top customer by revenue/money
        [Display(Name = "Top Customer - by revenue")]
        public string TopCustomerRevName {get; set;}

        [Display(Name = "Top Customer Revenue")]
        public decimal TopCustomerRevenue { get; set; }


        [Display(Name = "Total Products")]
        public int TotalProductsCount { get; set; }

        //momo or cash OR Card
        public int numOfMobileMoney { get; set;}
        public int numOfCash { get; set; }
        public int numOfCard { get; set; }


        // --- Top Products Chart ---
        public List<string> TopProductNames { get; set; } = new List<string>();
        public List<int> TopProductQuantities { get; set; } = new List<int>();


        // --- 2. The "Recent Activity" List ---
        // We can re-use the ViewModel we already built for the Sales/Index page!
        public List<SaleListViewModel> RecentSales { get; set; } = new List<SaleListViewModel>();



        // --- 3. Data for Charts ---
        public List<SalesOverTimeData> SalesOverTime { get; set; }

    }
}