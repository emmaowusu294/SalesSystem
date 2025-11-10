using Microsoft.AspNetCore.Mvc;
using SalesSystem.Services; // <-- We need this!
using SalesSystem.ViewModels; // <-- And this!
using System.Linq; // <-- And especially this for LINQ (.Sum(), .Count(), .Take())

namespace SalesSystem.Controllers
{
    public class DashboardController : Controller
    {
        // --- 1. Give it "brains" by injecting all three services ---
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly SaleService _saleService;

        public DashboardController(ProductService productService,
                                 CustomerService customerService,
                                 SaleService saleService)
        {
            _productService = productService;
            _customerService = customerService;
            _saleService = saleService;
        }

        // --- 2. The Index() action is its one and only job ---
        // This will run when the user goes to /Dashboard
        public IActionResult Index()
        {
            // --- Get all the raw data from your services ---
            var allSales = _saleService.GetAllSales();
            var allCustomers = _customerService.GetAllCustomers();
            var allProducts = _productService.GetAllProducts();

            // --- 3. Do the "Dashboard" math and build the "box" ---
            var viewModel = new DashboardViewModel
            {
                // a. The KPI Cards
                TotalRevenue = allSales.Sum(s => s.TotalAmount),
                TotalSalesCount = allSales.Count(),
                TotalCustomersCount = allCustomers.Count(),
                TotalProductsCount = allProducts.Count(),

                // b. The "Recent Activity" List
                // We'll just show the 5 most recent sales
                RecentSales = allSales.OrderByDescending(s => s.SaleDate)
                                      .Take(5)
                                      .ToList()
            };

            // --- 4. Pass the "box" of stats to the View ---
            return View(viewModel);
        }
    }
}