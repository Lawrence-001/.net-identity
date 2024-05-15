using Microsoft.EntityFrameworkCore;
using MVC.Identity.Data;
using MVC.Identity.Models;
using MVC.Identity.Services.Interfaces;

namespace MVC.Identity.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _context;

        public ProductService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product != null)
            {
                _context.Remove(product);
                _context.SaveChanges();
            }
            return null;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if (prod != null)
            {
                prod.Name = product.Name;
                prod.Price = product.Price;
                prod.Description = product.Description;
                prod.CreatedBy = product.CreatedBy;

                _context.Products.Update(prod);
                _context.SaveChanges();

                return product;
            }
            return null;

        }
    }
}
