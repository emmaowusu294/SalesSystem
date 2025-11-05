using SalesSystem.Data;
using SalesSystem.Models;
using SalesSystem.ViewModels;

namespace SalesSystem.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        //CRUD Operations

        //List all products
        public List<ProductViewModel> GetAllProducts() {
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

        //create a new product
        public void CreateProduct(ProductViewModel viewModel) {
            var product = new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                SellingPrice = viewModel.SellingPrice,
                StockQuantity = viewModel.StockQuantity,
            };

            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // update
        public void UpdateProduct(ProductViewModel viewModel) { 
            var product = _context.Products.Find(viewModel.ProductId);

            if (product != null) { 
                product.Name = viewModel.Name;
                product.Description = viewModel.Description;
                product.SellingPrice = viewModel.SellingPrice;
                product.StockQuantity = viewModel.StockQuantity;

                _context.Products.Update(product);
                _context.SaveChanges();
            }
        }

        // Get a single product by its ID
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

        public void DeleteProduct(int id) {
            var product = _context.Products.Find(id);

            if (product != null) { 
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
