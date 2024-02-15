using Servindustria.Models;

namespace Servindustria.Data.Interfaces;
public interface IProductCategoryRepository {
    Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync();
    Task<ProductCategory?> GetProductCategoryByIdAsync(int id);
    Task<ProductCategory> AddProductCategoryAsync(ProductCategory productCategory);
    Task<ProductCategory> UpdateProductCategoryAsync(ProductCategory productCategory);
    Task<bool> DeleteProductCategoryAsync(int id);
}