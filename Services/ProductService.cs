using SalesSystem.Data;
using SalesSystem.Helpers;
using SalesSystem.Models;
using SalesSystem.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SalesSystem.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }



        public List<ProductViewModel> GetAllProducts()
        {
            var viewModels = _context.Products
                .Select(s => new ProductViewModel
                {
                    ProductId = s.ProductId,
                    Name = s.Name,
                    Description = s.Description,
                    SellingPrice = s.SellingPrice,
                    StockQuantity = s.StockQuantity,
                })
                .ToList();
            return viewModels;
        }

        public ProductViewModel GetProductById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return null;
            }
            var viewModel = new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                SellingPrice = product.SellingPrice,
                StockQuantity = product.StockQuantity,
            };
            return viewModel;
        }

       

        public ServiceResponse CreateProduct(ProductViewModel viewModel)
        {
            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                SellingPrice = viewModel.SellingPrice,
                StockQuantity = viewModel.StockQuantity,
            };

            _context.Products.Add(product);
            _context.SaveChanges();

     
            return new ServiceResponse
            {
                Success = true,
                Message = $"Product '{product.Name}' created successfully."
            };
        }

        public ServiceResponse UpdateProduct(ProductViewModel viewModel)
        {
            var product = _context.Products.Find(viewModel.ProductId);

            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product not found. Update failed."
                };
            }

            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.SellingPrice = viewModel.SellingPrice;
            product.StockQuantity = viewModel.StockQuantity;

            _context.Products.Update(product);
            _context.SaveChanges();

           
            return new ServiceResponse
            {
                Success = true,
                Message = $"Product '{product.Name}' updated successfully."
            };
        }

        public ServiceResponse DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Product not found. Delete failed."
                };
            }

            var productName = product.Name;
            _context.Products.Remove(product);
            _context.SaveChanges();

           
            return new ServiceResponse
            {
                Success = true,
                Message = $"Product '{productName}' deleted successfully."
            };
        }
    }
}