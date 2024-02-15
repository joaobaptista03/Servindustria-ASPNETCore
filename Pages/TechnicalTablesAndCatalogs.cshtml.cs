using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

public class TechnicalTableOrCatalogsModel : PageModel {
    public ITechnicalTableOrCatalogRepository _technicalTable;

    public IEnumerable<TechnicalTableOrCatalog> TechnicalTableOrCatalogs = new List<TechnicalTableOrCatalog>();

    public TechnicalTableOrCatalogsModel(ITechnicalTableOrCatalogRepository technicalTable)
    {
        _technicalTable = technicalTable;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        TechnicalTableOrCatalogs = await _technicalTable.GetTechnicalTableOrCatalogsAsync();
        return Page();
    }
}