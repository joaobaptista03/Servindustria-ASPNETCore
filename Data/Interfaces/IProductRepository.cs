using Servindustria.Models;

namespace Servindustria.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<(IEnumerable<Product> products, int totalCount)> GetProductsAsync(int currentPage, int pageSize);
    }
}
