using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesSystem.Services;
using SalesSystem.ViewModels;
using System.Linq;

namespace SalesSystem.Controllers
{
    public class SalesController : Controller
    {
        private readonly SaleService _saleService;
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;

        public SalesController(SaleService saleService,
                               CustomerService customerService,
                               ProductService productService)
        {
            _saleService = saleService;
            _customerService = customerService;
            _productService = productService;
        }


        // GET: /Sales
        public ActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Success = TempData["Success"];
            var allSales = _saleService.GetAllSales();
            return View(allSales);
        }

        // GET: /Sales/Create
        public ActionResult Create()
        {
            //  just need to get the customer list for the dropdown
            var customers = _customerService.GetAllCustomers()
                    .OrderBy(c => c.Name);

            var viewModel = new SaleCreateDraftViewModel
            {
                CustomerList = new SelectList(customers, "CustomerId", "Name")
            };
            return View(viewModel); // Goes to Create.cshtml
        }


        // POST: /Sales/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SaleCreateDraftViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Call new, simple service method
                var response = _saleService.CreateSaleDraft(viewModel.CustomerId, viewModel.PaymentMethod);

                if (response.Success)
                {
                    // The "draft" is created. Now redirect to the
                    // "Add Items" (Details) page for this new Sale.
                    return RedirectToAction(nameof(Details), new { id = response.Data.SaleId });
                }
                else
                {
                    // Failed (e.g., customer not found)
                    ModelState.AddModelError(string.Empty, response.Message);
                }
            }

            // the form was invalid. Re-populate the dropdown.
            var customers = _customerService.GetAllCustomers()
                    .OrderBy(c => c.Name);
            viewModel.CustomerList = new SelectList(customers, "CustomerId", "Name");
            return View(viewModel);
        }

        // add items page
        // GET: /Sales/Details/5
        public ActionResult Details(int id)
        {
            // This is our main "hub" page.
            // It needs to show two things:
            // 1. The receipt details (what's in the cart so far)
            // 2. The "Add Item" form

            // 1. Get the receipt details
            var receiptViewModel = _saleService.GetSaleDetails(id);
            if (receiptViewModel == null)
            {
                TempData["Message"] = "Sale not found. Invalid ID.";
                TempData["Success"] = false;
                return RedirectToAction(nameof(Index));
            }

            // 2. Create the "Add Item" form's ViewModel
            var allProducts = _productService.GetAllProducts().Where(p => p.StockQuantity > 0)
                .OrderBy(p => p.Name);
            var addItemViewModel = new SaleAddItemViewModel
            {
                SaleId = id, // <-- Pre-fill the hidden SaleId
                ProductList = new SelectList(allProducts, "ProductId", "Name")
            };

            // 3. Create a "wrapper" ViewModel to pass BOTH to the view
            var wrapper = new SaleDetailWrapperViewModel
            {
                Receipt = receiptViewModel,
                AddItemForm = addItemViewModel
            };

            // 4. Pass any "Item Added" messages to the view
            ViewBag.Message = TempData["Message"];
            ViewBag.Success = TempData["Success"];

            return View(wrapper); // Goes to Details.cshtml
        }

      
        // This is a new action that the "Add Item" form will post to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(SaleAddItemViewModel AddItemForm)
        {
            if (ModelState.IsValid)
            {
                var response = _saleService.AddItemToSale(AddItemForm.SaleId, AddItemForm.ProductId, AddItemForm.Quantity);

                // Set a message (success or failure)
                TempData["Message"] = response.Message;
                TempData["Success"] = response.Success;
            }
            else
            {
                TempData["Message"] = "Invalid quantity or product.";
                TempData["Success"] = false;
            }

            //redirect back to the same page
            return RedirectToAction(nameof(Details), new { id = AddItemForm.SaleId });
        }


        // This is a new action for the "Finalize" button
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Finalize(int saleId)
        {
            var response = _saleService.FinalizeSale(saleId);

            TempData["Message"] = response.Message;
            TempData["Success"] = response.Success;

            // After finalizing, go to the Sales History
            return RedirectToAction(nameof(Index));
        }
    }
}