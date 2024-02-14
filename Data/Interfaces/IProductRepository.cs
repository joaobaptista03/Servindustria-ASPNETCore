using Servindustria.Models;

namespace Servindustria.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> products, int totalCount)> GetProductsAsync(int currentPage, int pageSize, string search, int filter);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
