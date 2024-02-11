using Servindustria.Models;
namespace Servindustria.Data.Interfaces;

public interface ITechnicalTableRepository {
    Task<IEnumerable<TechnicalTable>> GetTechnicalTablesAsync();
    Task DeleteTechnicalTableAsync(int id);
    Task AddTechnicalTableAsync(TechnicalTable technicalTable);
}