using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

namespace Servindustria.Pages
{
    public class ManageItemsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ITechnicalTableRepository _technicalTableRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ManageItemsModel(IProductRepository productRepository, ITechnicalTableRepository technicalTableRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _technicalTableRepository = technicalTableRepository;
            _productCategoryRepository = productCategoryRepository;
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<TechnicalTable> TechnicalTables { get; set; } = new List<TechnicalTable>();
        public IEnumerable<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
            if (!User.IsInRole("Admin")) return RedirectToPage("/Index");
            Products = await _productRepository.GetAllProductsAsync();
            TechnicalTables = await _technicalTableRepository.GetTechnicalTablesAsync();
            ProductCategories = await _productCategoryRepository.GetProductCategoriesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddProductAsync(Product? Product = null, IFormFile? ImageFile = null, IFormFile? PdfFile = null) {
            if (!ModelState.IsValid || Product == null || ImageFile == null || PdfFile == null) return Page();

            ProductCategory? category = await _productCategoryRepository.GetProductCategoryByIdAsync(Product.CategoryId);
            if (category == null) return Page();
            Product.Category = category;

            await _productRepository.AddProductAsync(Product);
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs", "product_" + Product.Id.ToString() + Path.GetExtension(ImageFile.FileName));
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "product_" + Product.Id.ToString() + Path.GetExtension(PdfFile.FileName));

            if (ImageFile != null)
            {
                using (var fileStream = new FileStream(imgPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }
            }

            if (PdfFile != null)
            {
                using (var fileStream = new FileStream(pdfPath, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(fileStream);
                }
            }

            TempData["SuccessMessage"] = "Produto adicionado com sucesso.";
            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostAddTechnicalTableAsync(TechnicalTable? TechnicalTable = null, IFormFile? PdfFile = null) {
            if (!ModelState.IsValid || TechnicalTable == null || PdfFile == null) return Page();

            await _technicalTableRepository.AddTechnicalTableAsync(TechnicalTable);
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "technicaltable_" + TechnicalTable.Id.ToString() + Path.GetExtension(PdfFile.FileName));

            if (PdfFile != null)
            {
                using (var fileStream = new FileStream(pdfPath, FileMode.Create))
                {
                    await PdfFile.CopyToAsync(fileStream);
                }
            }

            TempData["SuccessMessage"] = "Tabela técnica adicionada com sucesso.";
            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(int id) {
            await _productRepository.DeleteProductAsync(id);
            
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs", "product_" + id.ToString() + getProductImageExtension(id));
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "product_" + id.ToString() + ".pdf");

            if (System.IO.File.Exists(imgPath)) System.IO.File.Delete(imgPath);
            if (System.IO.File.Exists(pdfPath)) System.IO.File.Delete(pdfPath);

            TempData["SuccessMessage"] = "Produto eliminado com sucesso.";
            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostDeleteTechnicalTableAsync(int id) {
            await _technicalTableRepository.DeleteTechnicalTableAsync(id);

            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "technicaltable_" + id.ToString() + ".pdf");
            if (System.IO.File.Exists(pdfPath)) System.IO.File.Delete(pdfPath);

            TempData["SuccessMessage"] = "Tabela técnica eliminada com sucesso.";
            return RedirectToPage("/AdminManageItems");
        }

        private string getProductImageExtension(int id) {
            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs"))) {
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith("product_" + id.ToString())) {
                    return Path.GetExtension(fileName);
                }
            }

            return "";
        }

        public async Task<IActionResult> OnPostAddProductCategoryAsync(ProductCategory? ProductCategory = null) {
            if (!ModelState.IsValid || ProductCategory == null) return Page();

            await _productCategoryRepository.AddProductCategoryAsync(ProductCategory);
            TempData["SuccessMessage"] = "Categoria de produto adicionada com sucesso.";
            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostDeleteProductCategoryAsync(int id) {
            bool result = await _productCategoryRepository.DeleteProductCategoryAsync(id);
            if (!result) {
                TempData["ErrorMessage"] = "Não pode eliminar uma categoria de produto enquanto houver produtos associados a ela.";
            }
            else {
                TempData["SuccessMessage"] = "Categoria de produto eliminada com sucesso.";
            }
            return RedirectToPage("/AdminManageItems");
        }
    }
}
