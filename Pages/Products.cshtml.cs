using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

public class ProductsModel : PageModel {
    public IProductRepository _productRepository;

    public ProductsModel(IProductRepository productRepository) {
        _productRepository = productRepository;
    }

    public IEnumerable<Product>? Products;
    public int TotalProducts { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task<IActionResult> OnGetAsync(int currentPage = 1)
    {
        CurrentPage = currentPage;

        if (CurrentPage < 1)
        {
            CurrentPage = 0;
            return RedirectToPage("/Products", new { CurrentPage });
        }

        var products = await _productRepository.GetProductsAsync(CurrentPage, PageSize);

        TotalProducts = products.totalCount;
        Products = products.products;

        if (CurrentPage > (TotalProducts / PageSize + 1))
        {
            CurrentPage = (int)Math.Ceiling((double)TotalProducts / PageSize * 100) / 100;
            return RedirectToPage("/Products", new { CurrentPage });
        }

        if (CurrentPage > 1 && Products.Count() == 0)
        {
            CurrentPage--;
            return RedirectToPage("/Products", new { CurrentPage });
        }

        return Page();
    }

    public IActionResult OnPostPage(int CurrentPage)
    {
        return RedirectToPage("/Products", new { CurrentPage });
    }
}