using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var viewModel = _productService.GetAllProducts();
            return View(viewModel);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {   
            var viewModel = _productService.GetProductById(id);
            if (viewModel == null) { 
                return NotFound();
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
        public ActionResult Create(ProductViewModel viewModel, IFormCollection collection)
        {
            if (ModelState.IsValid) 
            {
                _productService.CreateProduct(viewModel);

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var viewModel = _productService.GetProductById(id);
            if (viewModel == null) {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,ProductViewModel viewModel ,IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                _productService.UpdateProduct(viewModel);
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
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                _productService.DeleteProduct(id);

                return RedirectToAction(nameof(Index));

            }
            return View();
        }
    }
}
