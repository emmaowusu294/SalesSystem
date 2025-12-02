using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Data;
using SalesSystem.Models;
using SalesSystem.Services; 
using SalesSystem.ViewModels; 
using System.Linq; 

namespace SalesSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly SaleService _saleService;
        private readonly ApplicationDbContext _context;

        public DashboardController(ProductService productService,
                                 CustomerService customerService,
                                 SaleService saleService,
                                 ApplicationDbContext context)
        {
            _productService = productService;
            _customerService = customerService;
            _saleService = saleService;
            _context = context;
        }


        public IActionResult Index()
        {

            var allSales = _saleService.GetAllSales();
            var allCustomers = _customerService.GetAllCustomers();
            var allProducts = _productService.GetAllProducts();


            // customer with the most sales
            var topCustomer = _context.Sales
            .GroupBy(s => s.CustomerId)
            .Select(g => new { CustomerId = g.Key, SalesCount = g.Count() })
            .OrderByDescending(x => x.SalesCount)
            .FirstOrDefault();

            var topCustomerName = topCustomer != null
                ? _context.Customers
                    .FirstOrDefault(c => c.CustomerId == topCustomer.CustomerId)?.Name
                : "N/A";

            // customer with the most revenue
            var topCustomerRev = _context.Sales
           .GroupBy(s => s.CustomerId)
           .Select(g => new { CustomerId = g.Key, totalRevenue = g.Sum(t => t.TotalAmount) })
           .OrderByDescending(x => x.totalRevenue)
           .FirstOrDefault();

            var topCustomerRevName = topCustomerRev != null
               ? _context.Customers
                   .FirstOrDefault(c => c.CustomerId == topCustomerRev.CustomerId)?.Name
               : "N/A";

            //logic for top products 
            // 1. Group products by sales quantity
            var productSales = _context.SaleItems
                .GroupBy(s => s.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalQty = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQty)
                .ToList();

            // 2. Get Top 5
            var top5 = productSales.Take(5).ToList();

            // 3. Sum everything else as "Others"
            var othersSum = productSales.Skip(5).Sum(x => x.TotalQty);

            if (othersSum > 0)
            {
                top5.Add(new { ProductName = "Others", TotalQty = othersSum });
            }




            var today = DateTime.Today;

            var todaySales = _context.Sales
                 .Include(s => s.SaleItems)          
                    .ThenInclude(si => si.Product)  
                 .Where(s => s.SaleDate >= today && s.SaleDate < today.AddDays(1))
                 .ToList();


            //today's stats
            var todayRevenue = todaySales.Sum(s => s.TotalAmount);
            var todaySalesCount = todaySales.Count();


            var todayTopProduct = todaySales
                .SelectMany(s => s.SaleItems)
                .GroupBy(si => si.Product)
                .Select(g => new
                {
                    ProductName = g.Key.Name,
                    TotalQty = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQty)
                .FirstOrDefault();


            //weekly stats--------------------------///
            // weekly range logic
            //calculating how many days to subtract to get back to Monday
            int daysDifference = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

            // If today is Sunday (daysDifference would be -1), adjust to go back a full week
            if (daysDifference < 0)
            {
                daysDifference += 7;
            }


            DateTime mondayDate = today.AddDays(-daysDifference);
            DateTime saturdayDate = mondayDate.AddDays(5); // 5 days after Monday is Saturday
            // Format the dates as strings (dd/MM/yyyy )
            string mondayFormatted = mondayDate.ToString("dd/MM/yyyy");
            string saturdayFormatted = saturdayDate.ToString("dd/MM/yyyy");

            //main weekly stats
            var weeklySales = _context.Sales
                 .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                 .Where(s => s.SaleDate >= mondayDate && s.SaleDate < saturdayDate)
                 .ToList();

            var weekRevenue = weeklySales.Sum(w => w.TotalAmount);
            var weekSalesCount = weeklySales.Count();

            var weekTopProduct = weeklySales
                .SelectMany(w => w.SaleItems)
                .GroupBy(s => s.Product)
                .Select(g => new
                {
                    ProductName = g.Key.Name,
                    TotalQty = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalQty)
                .FirstOrDefault();


            //monthly stats
            var currentMonth = today.Month;
            Console.WriteLine($"Current Month: {currentMonth}");

            var monthlySales = _context.Sales
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Where(s => s.SaleDate.Month == currentMonth);

            var monthRevenue = monthlySales.Sum(m => m.TotalAmount);
            var monthSalesCount = monthlySales.Count();

            var monthTopProduct = monthlySales
                .SelectMany (s => s.SaleItems)
                .GroupBy(s => s.Product)
                .Select(g => new
                {
                    ProductName = g.Key.Name,
                    TotalQty = g.Sum(x => x.Quantity)
                }).OrderByDescending(x => x.TotalQty)
                .FirstOrDefault();

            var viewModel = new DashboardViewModel
            {
                // a. The Cards
                TotalRevenue = allSales.Sum(s => s.TotalAmount),
                TotalSalesCount = allSales.Count(),
                TotalCustomersCount = allCustomers.Count(),
                TotalProductsCount = allProducts.Count(),

                TopCustomerName = topCustomerName,
                TopCustomerSalesCount = topCustomer?.SalesCount ?? 0,

                TopCustomerRevName = topCustomerRevName,
                TopCustomerRevenue = topCustomerRev?.totalRevenue ?? 0,

                //the 5 most recent sales
                RecentSales = allSales.OrderByDescending(s => s.SaleDate)
                                      .Take(5)
                                      .ToList(),

                //momo or cash
                numOfMobileMoney = allSales.Count(a => a.PaymentMethod == PaymentMethod.MobileMoney),

                numOfCash = allSales.Count(a => a.PaymentMethod == PaymentMethod.Cash),

                numOfCard = allSales.Count(a => a.PaymentMethod == PaymentMethod.Card),

                TopProductNames = top5.Select(x => x.ProductName).ToList(),

                TopProductQuantities = top5.Select(x => x.TotalQty).ToList(),

                //today's stats
                TodayRevenue = todayRevenue,
                TodaySalesCount = todaySalesCount,
                TodayTopProduct = todayTopProduct?.ProductName ?? "No sales today",
                TodayTopProductQuantity = todayTopProduct?.TotalQty ?? 0,

                //week's stat's
                WeekRange = $"Monday {mondayFormatted} - Saturday {saturdayFormatted}",
                WeekSalesCount = weekSalesCount,
                WeekRevenue = weekRevenue,
                WeekTopProduct = weekTopProduct?.ProductName ?? "No Sales this week",
                WeekTopProductQuantity = weekTopProduct?.TotalQty ?? 0,

                //Month's stat's
                MonthSalesCount = monthSalesCount,
                MonthRevenue = monthRevenue,
                MonthTopProduct = monthTopProduct?.ProductName ?? "No Sales this week",
                MonthTopProductQuantity = monthTopProduct?.TotalQty ?? 0,









                //for graph
                SalesOverTime = _context.Sales
                    .GroupBy(s => s.SaleDate.Date)
                    .Select(g => new SalesOverTimeData
                    {
                        Date = g.Key,
                        TotalSales = g.Sum(s => s.TotalAmount)
                    })
                    .OrderBy(x => x.Date)
                    .ToList()
            };

            return View(viewModel);
        }
    }
}