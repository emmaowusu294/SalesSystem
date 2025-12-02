using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    public class DashboardViewModel
    {

        [Display(Name = "Total Revenue")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalRevenue { get; set; }

        [Display(Name = "Total Sales")]
        public int TotalSalesCount { get; set; }

        [Display(Name = "Total Customers")]
        public int TotalCustomersCount { get; set; }

        //top customer with more sales
        [Display(Name = "Top Customer - by sales")]
        public string? TopCustomerName { get; set; }

        [Display(Name = "Top Customer Sales")]
        public int TopCustomerSalesCount { get; set; }

        //top customer by revenue/money
        [Display(Name = "Top Customer - by revenue")]
        public string? TopCustomerRevName {get; set;}

        [Display(Name = "Top Customer Revenue")]
        public decimal TopCustomerRevenue { get; set; }


        [Display(Name = "Total Products")]
        public int TotalProductsCount { get; set; }


        ////Top category
        //public string? TopCategoryName { get; set; }
        //public int? TopCategoryQty { get; set; }

        //momo or cash OR Card
        public int numOfMobileMoney { get; set;}
        public int numOfCash { get; set; }
        public int numOfCard { get; set; }



        // Today stats
        [Display(Name = "Today's Revenue")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TodayRevenue { get; set; }

        [Display(Name = "Today's Sales Count")]
        public int TodaySalesCount { get; set; }

        [Display(Name = "Today's Top Product")]
        public string TodayTopProduct { get; set; }
        public int TodayTopProductQuantity { get; set; }

        // Weekly stats
        public string WeekRange { get; set; }
        public decimal WeekRevenue { get; set; }
        public int WeekSalesCount { get; set; }
        public string WeekTopProduct { get; set; }
        public int WeekTopProductQuantity { get; set; }


        // Monthly stats
        //public string MonthRange { get; set; }
        public decimal MonthRevenue { get; set; }
        public int MonthSalesCount { get; set; }
        public string MonthTopProduct { get; set; }
        public int MonthTopProductQuantity { get; set; }




        // --- Top Products Chart ---
        public List<string> TopProductNames { get; set; } = new List<string>();
        public List<int> TopProductQuantities { get; set; } = new List<int>();


        // --- 2. The "Recent Activity" List ---
        // We can re-use the ViewModel we already built for the Sales/Index page!
        public List<SaleListViewModel> RecentSales { get; set; } = new List<SaleListViewModel>();



        // --- 3. Data for Charts ---
        public List<SalesOverTimeData>? SalesOverTime { get; set; }

    }
}