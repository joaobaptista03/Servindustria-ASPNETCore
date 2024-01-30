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

        public async Task<(IEnumerable<Product> products, int totalCount)> GetProductsAsync(int currentPage, int pageSize)
        {
            var products = _context.Products
                .OrderBy(p => p.Name)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            int totalCount = _context.Products.Count();

            return (await products.ToListAsync(), totalCount);
        }
    }
}
