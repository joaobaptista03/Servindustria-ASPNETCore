using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

public class ProductsModel : PageModel {
    public IProductRepository _productRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductsModel(IProductRepository productRepository, IWebHostEnvironment environment)
    {
        _productRepository = productRepository;
        _environment = environment;
    }

    public IEnumerable<Product>? Products;
    public int TotalProducts { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = 5;

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

    public string GetImagePathForProductId(int productId)
    {
        var basePath = Path.Combine(_environment.WebRootPath, "imgs");
        var searchPattern = $"product_{productId}.*";
        var files = Directory.GetFiles(basePath, searchPattern);

        if (files.Length > 0)
        {
            var fileName = Path.GetFileName(files[0]);
            return $"/imgs/{fileName}";
        }

        return "error";
    }
}