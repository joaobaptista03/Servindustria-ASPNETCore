using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

public class ProductsModel : PageModel {
    public IProductRepository _productRepository;
    public IProductCategoryRepository _productCategoryRepository;
    private readonly IWebHostEnvironment _environment;

    public ProductsModel(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, IWebHostEnvironment environment)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
        _environment = environment;
    }

    public IEnumerable<Product>? Products;
    public int TotalProducts { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; } = 5;
    
    [FromQuery]
    public string Search { get; set; } = string.Empty;
    
    [FromQuery]
    public int Filter { get; set; } = -1;
    public string FilterName { get; set; } = "Todos";
    
    public IEnumerable<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public async Task<IActionResult> OnGetAsync(int currentPage = 1)
    {
        CurrentPage = currentPage;

        if (CurrentPage < 1)
        {
            CurrentPage = 0;
            return RedirectToPage("/Products", new { CurrentPage, Search, Filter });
        }

        ProductCategories = await _productCategoryRepository.GetProductCategoriesAsync();
        if (Filter != -1)
        {
            FilterName = (await _productCategoryRepository.GetProductCategoryByIdAsync(Filter)).Name;
        }

        var products = await _productRepository.GetProductsAsync(CurrentPage, PageSize, Search, Filter);

        TotalProducts = products.totalCount;
        Products = products.products;

        if (CurrentPage > (TotalProducts / PageSize + 1))
        {
            CurrentPage = (int)Math.Ceiling((double)TotalProducts / PageSize * 100) / 100;
            return RedirectToPage("/Products", new { CurrentPage, Search, Filter });
        }

        if (CurrentPage > 1 && Products.Count() == 0)
        {
            CurrentPage--;
            return RedirectToPage("/Products", new { CurrentPage, Search, Filter });
        }

        return Page();
    }

    public IActionResult OnPostPage(int currentPage, string search, int filter)
    {
        return RedirectToPage("/Products", new { CurrentPage = currentPage, Search = search, Filter = filter });
    }

    public IActionResult OnPostSearch(string search, int filter)
    {
        
        return RedirectToPage("/Products", new { Search = search, Filter = filter });
    }

    public IActionResult OnPostFilter(int filter, string search) {
        return RedirectToPage("/Products", new { Filter = filter, Search = search });
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