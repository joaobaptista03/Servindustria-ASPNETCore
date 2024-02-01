using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servindustria.Data.Interfaces;
using Servindustria.Models;

namespace Servindustria.Pages
{
    public class AddProductModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public AddProductModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult OnGet()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("/Authentication");
            if (!User.IsInRole("Admin")) return RedirectToPage("/Index");

            return Page();
        }   

        [BindProperty]
        public Product Product { get; set; } = new Product();
        [BindProperty]
        public IFormFile ImageFile { get; set; }
        [BindProperty]
        public IFormFile PdfFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            return RedirectToPage("/AdminAddProduct");
        }
    }
}
