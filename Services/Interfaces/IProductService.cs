using MVC.Identity.Models;

namespace MVC.Identity.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product product);
        Task<Product> GetProductById(int id);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
        Task<IEnumerable<Product>> GetAllProducts();
    }
}
