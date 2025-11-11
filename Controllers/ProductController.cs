using Microsoft.AspNetCore.Mvc;
using SalesSystem.Helpers; 
using SalesSystem.Services; 
using SalesSystem.ViewModels; 

namespace SalesSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // GET: ProductController
        public ActionResult Index()
        {
            
            ViewBag.Message = TempData["Message"];
            ViewBag.Success = TempData["Success"];

            var viewModel = _productService.GetAllProducts();
            return View(viewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var viewModel = _productService.GetProductById(id);
            if (viewModel == null)
            {
                //return RedirectToAction(nameof(Details), new { id = 11 });
                var dummyViewModel =  new ProductViewModel
                {
                    ProductId = 0,
                    Name = "",
                    Description = "",
                    SellingPrice = 0,
                    StockQuantity = 0,
                };
                ViewBag.Message = "Product not found.";
                ViewBag.Success = false;
                return View(dummyViewModel);
            }
            return View(viewModel);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                
                var response = _productService.CreateProduct(viewModel);

                
                if (!response.Success)
                {
                    
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(viewModel); 
                }

                TempData["Message"] = response.Message;
                TempData["Success"] = response.Success;
                return RedirectToAction(nameof(Index));
            }

            
            return View(viewModel);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _productService.GetProductById(id);
            if (viewModel == null)
            {
                var dummyViewModel = new ProductViewModel
                {
                    ProductId = 0,
                    Name = "",
                    Description = "",
                    SellingPrice = 0,
                    StockQuantity = 0,
                };
                ViewBag.Message = "Product not found.";
                ViewBag.Success = false;
                return View(dummyViewModel);
            }
            return View(viewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductViewModel viewModel)
        {
            if (id != viewModel.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var response = _productService.UpdateProduct(viewModel);

                
                if (!response.Success)
                {
                    
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(viewModel); 
                }

                
                TempData["Message"] = response.Message;
                TempData["Success"] = response.Success;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var viewModel = _productService.GetProductById(id);
            if (viewModel == null)
            {
                var dummyViewModel = new ProductViewModel
                {
                    ProductId = 0,
                    Name = "",
                    Description = "",
                    SellingPrice = 0,
                    StockQuantity = 0,
                };
                ViewBag.Message = "Product not found.";
                ViewBag.Success = false;
                return View(dummyViewModel);
            }
            return View(viewModel);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            var response = _productService.DeleteProduct(id);

            TempData["Message"] = response.Message;
            TempData["Success"] = response.Success;

            return RedirectToAction(nameof(Index));
        }
    }
}