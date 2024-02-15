using Microsoft.EntityFrameworkCore;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

namespace Servindustria.Data.Components {
    public class TechnicalTableOrCatalogRepository : ITechnicalTableOrCatalogRepository {
        private readonly ServindustriaDBContext _context;

        public TechnicalTableOrCatalogRepository(ServindustriaDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TechnicalTableOrCatalog>> GetTechnicalTableOrCatalogsAsync() {
            return await _context.TechnicalTableOrCatalogs.ToListAsync();
        }

        public async Task DeleteTechnicalTableOrCatalogAsync(int id) {
            var technicalTable = await _context.TechnicalTableOrCatalogs.FindAsync(id);
            if (technicalTable == null) return;
            _context.TechnicalTableOrCatalogs.Remove(technicalTable);
            await _context.SaveChangesAsync();
        }

        public async Task AddTechnicalTableOrCatalogAsync(TechnicalTableOrCatalog technicalTable) {
            _context.TechnicalTableOrCatalogs.Add(technicalTable);
            await _context.SaveChangesAsync();
        }
    }
}