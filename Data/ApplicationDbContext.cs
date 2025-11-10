using Microsoft.EntityFrameworkCore;
using SalesSystem.Models; 
using SalesSystem.ViewModels;

namespace SalesSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<SalesSystem.ViewModels.ProductViewModel> ProductViewModel { get; set; } = default!;
        public DbSet<SalesSystem.ViewModels.CustomerViewModel> CustomerViewModel { get; set; } = default!;
    }
}