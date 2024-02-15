using Servindustria.Models;
namespace Servindustria.Data.Interfaces;

public interface ITechnicalTableOrCatalogRepository {
    Task<IEnumerable<TechnicalTableOrCatalog>> GetTechnicalTableOrCatalogsAsync();
    Task DeleteTechnicalTableOrCatalogAsync(int id);
    Task AddTechnicalTableOrCatalogAsync(TechnicalTableOrCatalog technicalTable);
}