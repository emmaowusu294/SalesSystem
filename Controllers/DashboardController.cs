using Microsoft.AspNetCore.Mvc;
using SalesSystem.Data;
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

        // --- 2. The Index() action is its one and only job ---
        // This will run when the user goes to /Dashboard
        public IActionResult Index()
        {
            // --- Getting the raw data from the services ---
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