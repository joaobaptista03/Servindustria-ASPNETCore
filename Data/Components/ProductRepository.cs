using Microsoft.EntityFrameworkCore;
using Servindustria.Data.Interfaces;
using Servindustria.Models;
using System.Runtime.CompilerServices;

namespace Servindustria.Data.Components
{
    public class ProductRepository : IProductRepository
    {
        private readonly ServindustriaDBContext _context;

        public ProductRepository(ServindustriaDBContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Product> products, int totalCount)> GetProductsAsync(int currentPage, int pageSize, string search, int filter)
        {
            IQueryable<Product> products;
            int totalCount;

            if (search == string.Empty && filter == -1) {
                products = _context.Products
                    .OrderBy(p => p.Name)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize);
                totalCount = _context.Products.Count();
            } else if (search != string.Empty && filter == -1) {
                products = _context.Products
                    .Where(p => p.Name.Contains(search))
                    .OrderBy(p => p.Name)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize);
                totalCount = _context.Products.Count(p => p.Name.Contains(search));
            } else if (search == string.Empty && filter != -1) {
                products = _context.Products
                    .Where(p => p.CategoryId == filter)
                    .OrderBy(p => p.Name)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize);
                totalCount = _context.Products.Count(p => p.CategoryId == filter);
            } else {
                products = _context.Products
                    .Where(p => p.Name.Contains(search) && p.CategoryId == filter)
                    .OrderBy(p => p.Name)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize);
                totalCount = _context.Products.Count(p => p.Name.Contains(search) && p.CategoryId == filter);
            }

            return (await products.ToListAsync(), totalCount);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
