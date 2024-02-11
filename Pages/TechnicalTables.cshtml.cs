using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

public class TechnicalTablesModel : PageModel {
    public ITechnicalTableRepository _technicalTable;

    public IEnumerable<TechnicalTable> TechnicalTables = new List<TechnicalTable>();

    public TechnicalTablesModel(ITechnicalTableRepository technicalTable)
    {
        _technicalTable = technicalTable;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        TechnicalTables = await _technicalTable.GetTechnicalTablesAsync();
        return Page();
    }
}