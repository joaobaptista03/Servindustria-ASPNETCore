using Servindustria.Models;
using Servindustria.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data.Components;

public class ProductCategoryRepository : IProductCategoryRepository {
    private readonly ServindustriaDBContext _context;

    public ProductCategoryRepository(ServindustriaDBContext context) {
        _context = context;
    }

    public async Task<IEnumerable<ProductCategory>> GetProductCategoriesAsync() {
        return await _context.ProductCategories.ToListAsync();
    }

    public async Task<ProductCategory?> GetProductCategoryByIdAsync(int id) {
        return await _context.ProductCategories.FindAsync(id);
    }

    public async Task<ProductCategory> AddProductCategoryAsync(ProductCategory productCategory) {
        _context.ProductCategories.Add(productCategory);
        await _context.SaveChangesAsync();
        return productCategory;
    }

    public async Task<ProductCategory> UpdateProductCategoryAsync(ProductCategory productCategory) {
        _context.Entry(productCategory).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return productCategory;
    }

    public async Task DeleteProductCategoryAsync(int id) {
        var productCategory = await _context.ProductCategories.FindAsync(id);
        if (productCategory == null) return;

        _context.ProductCategories.Remove(productCategory);
        await _context.SaveChangesAsync();
    }
}