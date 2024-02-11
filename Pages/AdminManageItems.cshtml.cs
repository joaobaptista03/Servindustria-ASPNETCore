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

        public ManageItemsModel(IProductRepository productRepository, ITechnicalTableRepository technicalTableRepository)
        {
            _productRepository = productRepository;
            _technicalTableRepository = technicalTableRepository;
        }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<TechnicalTable> TechnicalTables { get; set; } = new List<TechnicalTable>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
            if (!User.IsInRole("Admin")) return RedirectToPage("/Index");
            Products = await _productRepository.GetAllProductsAsync();
            TechnicalTables = await _technicalTableRepository.GetTechnicalTablesAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddProductAsync(Product? Product = null, IFormFile? ImageFile = null, IFormFile? PdfFile = null) {
            if (!ModelState.IsValid || Product == null || ImageFile == null || PdfFile == null) return Page();

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

            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(int id) {
            await _productRepository.DeleteProductAsync(id);
            
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs", "product_" + id.ToString() + getImageExtension(id));
            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "product_" + id.ToString() + ".pdf");

            if (System.IO.File.Exists(imgPath)) System.IO.File.Delete(imgPath);
            if (System.IO.File.Exists(pdfPath)) System.IO.File.Delete(pdfPath);

            return RedirectToPage("/AdminManageItems");
        }

        public async Task<IActionResult> OnPostDeleteTechnicalTableAsync(int id) {
            await _technicalTableRepository.DeleteTechnicalTableAsync(id);

            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", "technicaltable_" + id.ToString() + ".pdf");
            if (System.IO.File.Exists(pdfPath)) System.IO.File.Delete(pdfPath);

            return RedirectToPage("/AdminManageItems");
        }

        private string getImageExtension(int id) {
            foreach (string file in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs"))) {
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith("product_" + id.ToString())) {
                    return Path.GetExtension(fileName);
                }
            }

            return "";
        }
    }
}
