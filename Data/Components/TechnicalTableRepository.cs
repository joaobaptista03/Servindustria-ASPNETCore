using Microsoft.EntityFrameworkCore;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

namespace Servindustria.Data.Components {
    public class TechnicalTableRepository : ITechnicalTableRepository {
        private readonly ServindustriaDBContext _context;

        public TechnicalTableRepository(ServindustriaDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<TechnicalTable>> GetTechnicalTablesAsync() {
            return await _context.TechnicalTables.ToListAsync();
        }

        public async Task DeleteTechnicalTableAsync(int id) {
            var technicalTable = await _context.TechnicalTables.FindAsync(id);
            if (technicalTable == null) return;
            _context.TechnicalTables.Remove(technicalTable);
            await _context.SaveChangesAsync();
        }

        public async Task AddTechnicalTableAsync(TechnicalTable technicalTable) {
            _context.TechnicalTables.Add(technicalTable);
            await _context.SaveChangesAsync();
        }
    }
}