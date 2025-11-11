using SalesSystem.Data;
using SalesSystem.Helpers;
using SalesSystem.Models;
using SalesSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SalesSystem.Services
{
    public class SaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// Creates a new "draft" Sale record. 
        public ServiceResponse<Sale> CreateSaleDraft(int customerId, PaymentMethod paymentMethod)
        {
            //  "ServiceResponse<T>" to return the new Sale object

            // Check if the customer exists
            var customer = _context.Customers.Find(customerId);
            if (customer == null)
            {
                return new ServiceResponse<Sale>
                {
                    Success = false,
                    Message = "Customer not found."
                };
            }

            var sale = new Sale
            {
                CustomerId = customerId,
                PaymentMethod = paymentMethod,
                SaleDate = DateTime.Now,
                TotalAmount = 0 // It's a draft, so the total is 0 for now
            };

            _context.Sales.Add(sale);
            _context.SaveChanges();

            // Return a "success" response with the new Sale object
            return new ServiceResponse<Sale>
            {
                Success = true,
                Message = "Sale draft created successfully.",
                Data = sale // Return the new Sale so I can get its ID
            };
        }

   
        /// Adds a single product (SaleItem) to an existing Sale.
        public ServiceResponse AddItemToSale(int saleId, int productId, int quantity)
        {
            // Use a transaction here, because we are
            // 1) Creating a SaleItem
            // 2) Updating Product stock
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var product = _context.Products.Find(productId);
                    var sale = _context.Sales.Find(saleId);

                    if (product == null)
                    {
                        return new ServiceResponse { Success = false, Message = "Product not found." };
                    }
                    if (sale == null)
                    {
                        return new ServiceResponse { Success = false, Message = "Sale not found." };
                    }

                    // Check for stock
                    if (product.StockQuantity < quantity)
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Message = $"Not enough stock for '{product.Name}'. Only {product.StockQuantity} available."
                        };
                    }

                    // Create the "line item"
                    var saleItem = new SaleItem
                    {
                        SaleId = saleId,
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = product.SellingPrice // This is the "freeze"
                    };
                    _context.SaleItems.Add(saleItem);



                    // Update the product's stock
                    product.StockQuantity -= quantity;

                    _context.SaveChanges();
                    transaction.Commit(); // Commit the changes

                    return new ServiceResponse { Success = true, Message = "Item added." };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new ServiceResponse { Success = false, Message = $"Failed to add item: {ex.Message}" };
                }
            }
        }


        // Calculates the final total for a Sale and updates the master record.

        public ServiceResponse FinalizeSale(int saleId)
        {
            var sale = _context.Sales
                .Include(s => s.SaleItems) // Load all the items for this sale
                .FirstOrDefault(s => s.SaleId == saleId);

            if (sale == null)
            {
                return new ServiceResponse { Success = false, Message = "Sale not found." };
            }

            // Calculate the total
            decimal finalTotal = 0;
            foreach (var item in sale.SaleItems)
            {
                finalTotal += (item.UnitPrice * item.Quantity);
            }

            // Update the master sale record
            sale.TotalAmount = finalTotal;
            _context.SaveChanges();

            return new ServiceResponse
            {
                Success = true,
                Message = $"Sale #{saleId} finalized. Total: {finalTotal:C}"
            };
        }


 
        public List<SaleListViewModel> GetAllSales()
        {
            var sales = _context.Sales
                .Include(s => s.Customer)
                .OrderByDescending(s => s.SaleDate)
                .Select(s => new SaleListViewModel
                {
                    SaleId = s.SaleId,
                    SaleDate = s.SaleDate,
                    CustomerName = s.Customer.Name,
                    PaymentMethod = s.PaymentMethod,
                    TotalAmount = s.TotalAmount
                })
                .ToList();
            return sales;
        }

        public SaleDetailViewModel GetSaleDetails(int saleId)
        {
            var sale = _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .FirstOrDefault(s => s.SaleId == saleId);

            if (sale == null)
            {
                return null;
            }

            var viewModel = new SaleDetailViewModel
            {
                SaleId = sale.SaleId,
                SaleDate = sale.SaleDate,
                PaymentMethod = sale.PaymentMethod,
                TotalAmount = sale.TotalAmount, // Will be 0 if still a "draft"
                CustomerName = sale.Customer.Name,
                CustomerPhone = sale.Customer.PhoneNumber,
                CustomerEmail = sale.Customer.Email,
                Items = sale.SaleItems.Select(si => new SaleItemDetailViewModel
                {
                    ProductName = si.Product.Name,
                    Quantity = si.Quantity,
                    UnitPrice = si.UnitPrice,
                    LineTotal = si.Quantity * si.UnitPrice
                }).ToList()
            };
            return viewModel;
        }
    }
}