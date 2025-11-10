using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesSystem.Helpers;
using SalesSystem.Services;
using SalesSystem.ViewModels;

namespace SalesSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: CustomerController
        public ActionResult Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Message = TempData["Message"];
            var allCustomers = _customerService.GetAllCustomers();
            return View(allCustomers);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            var viewModel = _customerService.GetCustomerById(id);
            if (viewModel == null) {
                TempData["Success"] = false;
                TempData["Message"] = "Customer doesn't exist";

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var response = _customerService.CreateCustomer(viewModel);
                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(viewModel);
                }

                TempData["Success"] = response.Success;
                TempData["Message"] = response.Message;
                return RedirectToAction(nameof(Index));

            }

            return View(viewModel);
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null) {
                TempData["Success"] = false;
                TempData["Message"] = "Customer not found";

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerViewModel viewModel)
        {
            if (id != viewModel.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) { 
                var response = _customerService.UpdateCustomer(viewModel);

                if (!response.Success)
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    return View(viewModel);
                   
                }
                TempData["Success"] = response.Success;
                TempData["Message"] = response.Message;

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null)
            {
                TempData["Success"] = false;
                TempData["Message"] = "Customer not found";

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var response = _customerService.DeleteCustomer(id);

            TempData["Message"] = response.Message;
            TempData["Success"] = response.Success;

            return RedirectToAction(nameof(Index));
        }
    }
}
