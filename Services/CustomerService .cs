using SalesSystem.Data;
using SalesSystem.Models;
using SalesSystem.ViewModels;

namespace SalesSystem.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        //CRUD Operations

        //List all customers
        public List<CustomerViewModel> GetAllCustomers()
        {
            var viewModels = _context.Customers
                .Select(s => new CustomerViewModel
                {
                    CustomerId = s.CustomerId,
                    Name = s.Name,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email,
                })
                .ToList();

            return viewModels;
        }

        // Create a new customer
        public void CreateCustomer(CustomerViewModel viewModel)
        {
            var customer = new Customer
            {
                Name = viewModel.Name,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }


        // Get a single customer by Id
        public CustomerViewModel GetCustomerById(int id) {
            var customer = _context.Customers.Find(id);

            if (customer == null) {
               return null;
            }

            var viewModel = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
            };

            return viewModel;

        }

        // update a customer information
        public void UpdateCustomer(CustomerViewModel viewModel) { 
            var customer = _context.Customers.Find(viewModel.CustomerId);

            if (customer != null) { 
                customer.Name = viewModel.Name;
                customer.PhoneNumber = viewModel.PhoneNumber;
                customer.Email = viewModel.Email;

                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
        }

        //delete a customer
        public void DeleteCustomer(CustomerViewModel viewModel) {
            var customer = _context.Customers.Find(viewModel.CustomerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }
        
    }
}
